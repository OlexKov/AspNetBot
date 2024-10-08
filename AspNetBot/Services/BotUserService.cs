using AspNetBot.Entities;
using AspNetBot.Interafces;

namespace AspNetBot.Services
{
    public class BotUserService : IBotUserService
    {
        public Task<IEnumerable<BotUser>> getForNotification()
        {
            return Task.Run(IEnumerable<BotUser> () => []);
        }
    }
}
