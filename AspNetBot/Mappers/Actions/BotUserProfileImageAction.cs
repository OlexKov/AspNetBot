using AspNetBot.Entities;
using AspNetBot.Interafces;
using AspNetBot.Models;
using AutoMapper;

namespace AspNetBot.Mappers.Actions
{
    public class BotUserProfileImageAction(IImageService imageService) : IMappingAction<BotUserCreationModel, BotUser>
    {
        //private readonly IImageService imageService = imageService;
        public async void Process(BotUserCreationModel source, BotUser destination, ResolutionContext context)
        {
            if (source.ImageFile is not null)
            {
                destination.Image =  await imageService.SaveImageAsync(source.ImageFile);
            }
            else if (!String.IsNullOrEmpty(source.ImageUrl)) 
            {
                destination.Image = await imageService.SaveImageFromUrlAsync(source.ImageUrl);
            }
            destination.Image = destination.Image;
        }
    }
}
