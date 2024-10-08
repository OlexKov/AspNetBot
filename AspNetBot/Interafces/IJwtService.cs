using AspNetBot.Entities;
using System.Security.Claims;
using Telegram.Bot.Types;

namespace AspNetBot.Interafces
{
    public interface IJwtService
    {
        Task<string> CreateTokenAsync(BotUser user);
        Task<IEnumerable<Claim>> GetClaimsAsync(BotUser user);
    }
}
