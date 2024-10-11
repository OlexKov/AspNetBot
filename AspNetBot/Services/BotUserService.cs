using AspNetBot.DTO;
using AspNetBot.Entities;
using AspNetBot.Exceptions;
using AspNetBot.Helpers;
using AspNetBot.Interafces;
using AspNetBot.Specifications;
using AutoMapper;
using System.Net;


namespace AspNetBot.Services
{
    public class BotUserService : IBotUserService
    {
        private readonly IRepository<BotUser> userRepo;
        private readonly IImageService imageService;
        private readonly IMapper mapper;

        public BotUserService(IMapper mapper, IRepository<BotUser> userRepo,IImageService imageService)
        {
            this.mapper = mapper;
            this.userRepo = userRepo;
            this.imageService = imageService;
        }
      

        public async Task DeleteAsync(long chatId)
        {
            var user = await userRepo.GetItemBySpec(new BotUserSpecs.GetByChatId(chatId)) ??
                throw new HttpException("Invalid user chat id",HttpStatusCode.BadRequest) ;
            await userRepo.DeleteAsync(user);
            await userRepo.SaveAsync();
            if(!String.IsNullOrEmpty(user.Image))
                imageService.DeleteImage(user.Image);
        }

      
        public async Task<IEnumerable<BotUserDto>> GetAllAsync(bool tracking) => mapper.Map<IEnumerable<BotUserDto>>(await userRepo.GetListBySpec(new BotUserSpecs.GetAll(tracking)));
       

        public async Task<IEnumerable<BotUserDto>> GetAllByProfessionAsync(int professionId,bool tracking) => mapper.Map<IEnumerable<BotUserDto>>(await userRepo.GetListBySpec(new BotUserSpecs.GetByProfession(professionId, tracking)));


        public async Task<IEnumerable<BotUserDto>> GetAllByProfessionAsync(string professionName, bool tracking) => mapper.Map<IEnumerable<BotUserDto>>(await userRepo.GetListBySpec(new BotUserSpecs.GetByProfession(professionName,tracking)));
        
        public async Task<BotUserDto> GetByChatIdAsync(long botUserChatId,bool tracking)  => mapper.Map<BotUserDto>(await userRepo.GetItemBySpec( new BotUserSpecs.GetById(botUserChatId, tracking)));

        public  async Task<BotUserDto> GetByIdAsync(int botUserId, bool tracking) => mapper.Map<BotUserDto>(await userRepo.GetItemBySpec(new BotUserSpecs.GetById(botUserId, tracking)));

        public PaginationResult<BotUserDto, BotUser> GetPagination(int page, int size) => new(userRepo,mapper,page,size);
        

        public async Task<BotUserDto> SetUserProfessionAsync(long chatId, int professionId)
        {
            var botUser = await userRepo.GetItemBySpec(new BotUserSpecs.GetByChatId(chatId)) ??
               throw new HttpException("Invalid user chat  id", HttpStatusCode.BadRequest);
            botUser.ProfessionId = professionId;
            await userRepo.SaveAsync();
            return mapper.Map<BotUserDto>(botUser);
        }
    }
}
