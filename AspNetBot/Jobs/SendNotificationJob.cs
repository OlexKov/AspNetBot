using AspNetBot.Interafces;
using AspNetBot.Services;
using Quartz;

namespace AspNetBot.Jobs
{
    public class SendNotificationJob : IJob
    {
        private readonly IBotUserService userService;
        private readonly TelegarmBotService bot;

        public SendNotificationJob(IBotUserService userService, TelegarmBotService bot)
        {
            this.userService = userService;
            this.bot = bot;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var users = await userService.getForNotification();
            await bot.SendMessages(users); 
        }
    }
}
