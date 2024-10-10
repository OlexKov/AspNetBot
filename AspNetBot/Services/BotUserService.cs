using AspNetBot.DTO;
using AspNetBot.Entities;
using AspNetBot.Exceptions;
using AspNetBot.Helpers;
using AspNetBot.Interafces;
using AspNetBot.Models;
using AspNetBot.Specifications;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Net;
using Telegram.Bot.Types;

namespace AspNetBot.Services
{
    public class BotUserService : IBotUserService
    {
        private readonly IRepository<BotUser> userRepo;
        private readonly IMapper mapper;

        public BotUserService(IMapper mapper, IRepository<BotUser> userRepo)
        {
            this.mapper = mapper;
            this.userRepo = userRepo;
        }
      

        public async Task DeleteAsync(long chatId)
        {
            var user = await userRepo.GetItemBySpec(new BotUserSpecs.GetByChatId(chatId)) ??
                throw new HttpException("Invalid user chat id",HttpStatusCode.BadRequest) ;
            await userRepo.DeleteAsync(user);
            await userRepo.SaveAsync();
        }

      
        public async Task<IEnumerable<BotUserDto>> GetAllAsync(bool tracking) => mapper.Map<IEnumerable<BotUserDto>>(await userRepo.GetListBySpec(new BotUserSpecs.GetAll(tracking)));
       

        public async Task<IEnumerable<BotUserDto>> GetAllByProfessionAsync(int professionId,bool tracking) => mapper.Map<IEnumerable<BotUserDto>>(await userRepo.GetListBySpec(new BotUserSpecs.GetByProfession(professionId, tracking)));


        public async Task<IEnumerable<BotUserDto>> GetAllByProfessionAsync(string professionName, bool tracking) => mapper.Map<IEnumerable<BotUserDto>>(await userRepo.GetListBySpec(new BotUserSpecs.GetByProfession(professionName,tracking)));
        
        public async Task<BotUserDto> GetByChatIdAsync(long botUserChatId,bool tracking)  => mapper.Map<BotUserDto>(await userRepo.GetItemBySpec( new BotUserSpecs.GetById(botUserChatId, tracking)));

        public  async Task<BotUserDto> GetByIdAsync(int botUserId, bool tracking) => mapper.Map<BotUserDto>(await userRepo.GetItemBySpec(new BotUserSpecs.GetById(botUserId, tracking)));

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
