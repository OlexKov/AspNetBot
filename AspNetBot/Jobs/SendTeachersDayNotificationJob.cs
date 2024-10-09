using AspNetBot.Interafces;
using Quartz;
using Telegram.Bot;

namespace AspNetBot.Jobs
{
    public class SendTeachersDayNotificationJob : IJob
    {
        private readonly IBotUserService userService;
        private readonly ITelegramBotClient client;

        public SendTeachersDayNotificationJob(IBotUserService userService, IConfiguration config)
        {
            this.userService = userService;
            this.client = new TelegramBotClient(config["TelegramBotToken"]!);
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var users = await userService.getAllByProfession("Вчитель");
            foreach (var user in users.AsParallel())
            {
                await client.SendTextMessageAsync(user.UserId, $"{user.FirstName} {user.LastName} наша команда щиро вітає вас з Днем Вчителя та бажає всього найкращого !!!!");
            }
        }
    }
}
