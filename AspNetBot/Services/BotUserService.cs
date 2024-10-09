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
        private readonly UserManager<BotUser> userManager;
        private readonly IRepository<BotUser> userRepo;
        private readonly IMapper mapper;

        public BotUserService(UserManager<BotUser> userManager,IMapper mapper, IRepository<BotUser> userRepo)
        {
            this.userManager = userManager;
            this.mapper = mapper;
            this.userRepo = userRepo;
        }
        public async Task set(BotUserCreationModel model)
        {
            var botUser = mapper.Map<BotUser>(model);
            if (String.IsNullOrEmpty(model.Id))
            {
                var result = await userManager.CreateAsync(botUser);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(botUser, Roles.User.ToString());
                }
            }
               
            else 
                await userManager.UpdateAsync(botUser);
        }

        public async Task delete(BotUser botUser) => await userManager.DeleteAsync(botUser);
       

        public async Task delete(long botUserChatId)
        {
            var botUser = await userRepo.GetItemBySpec(new BotUserSpecs.GetByChatId(botUserChatId)) ??
                throw new HttpException("Invalid user chat id",HttpStatusCode.BadRequest) ;
            await delete(botUser);
        }

        public async Task delete(string botUserId)
        {
            var botUser = await userManager.FindByIdAsync(botUserId) ??
                throw new HttpException("Invalid user id", HttpStatusCode.BadRequest);
            await delete(botUser);
        }

        public async Task<IEnumerable<BotUserDto>> getAll(bool tracking) => mapper.Map<IEnumerable<BotUserDto>>(await userRepo.GetListBySpec(new BotUserSpecs.GetAll(tracking)));
       

        public async Task<IEnumerable<BotUserDto>> getAllByProfession(int professionId,bool tracking) => mapper.Map<IEnumerable<BotUserDto>>(await userRepo.GetListBySpec(new BotUserSpecs.GetByProfession(professionId, tracking)));


        public async Task<IEnumerable<BotUserDto>> getAllByProfession(string professionName, bool tracking) => mapper.Map<IEnumerable<BotUserDto>>(await userRepo.GetListBySpec(new BotUserSpecs.GetByProfession(professionName,tracking)));
        
        public async Task<BotUserDto> getByChatId(long botUserChatId,bool tracking)  => mapper.Map<BotUserDto>(await userRepo.GetItemBySpec( new BotUserSpecs.GetById(botUserChatId, tracking)));

        public  async Task<BotUserDto> getById(string botUserId, bool tracking) => mapper.Map<BotUserDto>(await userRepo.GetItemBySpec(new BotUserSpecs.GetById(botUserId, tracking)));

        public async Task<BotUserDto> setUserProfession(long userCatId, int professionId)
        {
            var botUser = await userRepo.GetItemBySpec(new BotUserSpecs.GetByChatId(userCatId)) ??
               throw new HttpException("Invalid user chat  id", HttpStatusCode.BadRequest);
            botUser.ProfessionId = professionId;
            await userRepo.SaveAsync();
            return mapper.Map<BotUserDto>(botUser);
        }
    }
}
