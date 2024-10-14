using AspNetBot.Exceptions;
using AspNetBot.Interafces;
using System.Net;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Webp;



namespace AspNetBot.Services
{
    public class ImageService :IImageService
    {
        private readonly IConfiguration config;
        private readonly string imgPath;

        public ImageService(IWebHostEnvironment env,IConfiguration config)
        {
            this.config = config;
            imgPath = Path.Combine(env.WebRootPath, config["UserImgDir"] ?? string.Empty);
            if (!Directory.Exists(imgPath))
            { 
                Directory.CreateDirectory(imgPath);
            }
        }

        public async Task<string> SaveImageAsync(IFormFile image)
        {
            using MemoryStream ms = new();
            await image.CopyToAsync(ms);
            return await SaveImageAsync(ms.ToArray());
        }

        public async Task<List<string>> SaveImagesAsync(IEnumerable<IFormFile> images)
        {
            var resultTasks = images.AsParallel().Select(x => SaveImageAsync(x));
            return [.. (await Task.WhenAll(resultTasks.ToArray()))];
        }

        public async Task<string> SaveImageAsync(string base64)
        {
            if (base64.Contains(','))
                base64 = base64.Split(',')[1];

            var bytes = Convert.FromBase64String(base64);

            var fileName = await SaveImageAsync(bytes);

            return fileName;
        }

        public async Task<string> SaveImageAsync(byte[] bytes)
        {
            List<int> sizes = config.GetRequiredSection("ImageSizes").Get<List<int>>()
                ?? throw new Exception("ImageSizes reading error");

            if (sizes.Count == 0)
                throw new Exception("ImageSizes not inicialized");

            string imageName = $"{Path.GetRandomFileName()}.webp";

            var tasks = sizes
                .AsParallel()
                .Select(s => SaveImageAsync(bytes, imageName, s))
                .ToArray();

            await Task.WhenAll(tasks);

            return imageName;
        }

        private async Task SaveImageAsync(byte[] bytes, string name, int size)
        {
            string dirSaveImage = Path.Combine(imgPath, $"{size}_{name}");

            using var image = Image.Load(bytes);
            try
            {
                image.Mutate(imageProcessingContext =>
                {
                    imageProcessingContext.Resize(new ResizeOptions
                    {
                        Size = new Size(Math.Min(image.Width, size), Math.Min(image.Height, size)),
                        Mode = ResizeMode.Max
                    });
                });

                using var stream = File.Create(dirSaveImage);
                await image.SaveAsync(stream, new WebpEncoder());
            }
            catch (Exception)
            {
                DeleteImageIfExists(dirSaveImage);
                throw new HttpException("Error image save", HttpStatusCode.InternalServerError);
            }
        }

        public async Task<List<string>> SaveImagesAsync(IEnumerable<byte[]> bytesArrays)
        {
            var resultTasks = bytesArrays.AsParallel().Select(x=> SaveImageAsync(x));
            return [.. (await Task.WhenAll(resultTasks.ToArray()))];
        }

        public async Task<byte[]> LoadBytesAsync(string name)
        {
            return await File.ReadAllBytesAsync(Path.Combine(imgPath, name));
        }

        public void DeleteImage(string nameWithFormat) => Sizes.AsParallel().ForAll(x=> File.Delete(Path.Combine(imgPath, $"{x}_{nameWithFormat}")));

        public void DeleteImages(IEnumerable<string> images) => images.AsParallel().ForAll(x => DeleteImage(x));
        
        public void DeleteImageIfExists(string nameWithFormat)
        {
            Sizes.AsParallel().ForAll(x => 
            {
                var path = Path.Combine(imgPath, $"{x}_{nameWithFormat}");
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            });
        }

        public void DeleteImagesIfExists(IEnumerable<string> images)
        {
            foreach (var image in images)
                DeleteImageIfExists(image);
        }

        public async Task<string> SaveImageFromUrlAsync(string imageUrl)
        {
            using var httpClient = new HttpClient();
            var imageBytes = await httpClient.GetByteArrayAsync(imageUrl);
            return await SaveImageAsync(imageBytes);
        }

        private List<int> Sizes
        {
            get
            {
                List<int> sizes = config.GetRequiredSection("ImageSizes").Get<List<int>>()
                ?? throw new HttpException("ImageSizes reading error", HttpStatusCode.InternalServerError);

                if (sizes.Count == 0)
                    throw new HttpException("ImageSizes not inicialized", HttpStatusCode.InternalServerError);
                return sizes;
            }
        }
    }
}

