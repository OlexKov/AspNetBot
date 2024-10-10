using AspNetBot.Entities;
using AspNetBot.Models;
using AspNetBot.Models.Account;

namespace AspNetBot.Interafces
{
    public interface IAccountService
    {
        Task<LoginResponse> LoginAsync(Models.Account.LoginRequest loginRequest);
        Task SetAsync(UserCreationModel model);
        Task DeleteAsync(int userId);
        Task DeleteAsync(BotUser user);

    }
}
