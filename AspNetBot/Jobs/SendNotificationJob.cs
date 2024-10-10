using AspNetBot.Interafces;
using Quartz;
using Telegram.Bot;

namespace AspNetBot.Jobs
{
    public class SendNotificationJob : IJob
    {
        private readonly IBotUserService userService;
        private readonly ITelegramBotClient client;

        public SendNotificationJob(IBotUserService userService, IConfiguration config)
        {
            this.userService = userService;
            this.client = new TelegramBotClient(config["TelegramBotToken"]!);
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var dataMap = context.JobDetail.JobDataMap;
            var profession = dataMap.GetString("profession");
            var message = dataMap.GetString("message");
            var users = await userService.getAllByProfession(profession);
            foreach (var user in users.AsParallel())
            {
                await client.SendTextMessageAsync(user.UserId, $"{user.FirstName} {user.LastName} {message}");
            }
        }
    }
}
