using AspNetBot.Entities;

namespace AspNetBot.Interafces
{
    public interface IBotUserService
    {
        Task<IEnumerable<BotUser>> getForNotification();
    }
}
