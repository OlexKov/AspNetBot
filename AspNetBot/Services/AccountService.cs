using AspNetBot.Entities;
using AspNetBot.Exceptions;
using AspNetBot.Helpers;
using AspNetBot.Interafces;
using AspNetBot.Models;
using AspNetBot.Models.Account;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace AspNetBot.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper mapper;
        private readonly UserManager<BotUser> userManager;
        private readonly IJwtService jwtService;

        public AccountService(IMapper mapper ,UserManager<BotUser> userManager,IJwtService jwtService)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.jwtService = jwtService;
        }
        public async Task SetAsync(UserCreationModel model)
        {
            var user = mapper.Map<BotUser>(model);
            if (model.Id == 0)
            {
                var result = await userManager.CreateAsync(user);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, Roles.User.ToString());
            }
            else
            {
                await userManager.UpdateAsync(user);
            }
        }

        public async Task DeleteAsync(int userId)
        {
            var botUser = await userManager.FindByIdAsync(userId.ToString()) ??
               throw new HttpException("Invalid user id", HttpStatusCode.BadRequest);
            await DeleteAsync(botUser); ;
        }

        
        public async Task DeleteAsync(BotUser user) => await userManager.DeleteAsync(user);

        public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
        {
            var user = await userManager.FindByEmailAsync(loginRequest.Email);
            if (user == null || !await userManager.CheckPasswordAsync(user, loginRequest.Password))
                throw new HttpException("Invalid register data", HttpStatusCode.BadRequest);
            return new(){Token = await jwtService.CreateTokenAsync(user)};
        }
    }
}
