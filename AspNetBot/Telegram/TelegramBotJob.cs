using AspNetBot.Helpers;
using AspNetBot.Interafces;
using Quartz;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace AspNetBot.Telegram
{
    public partial class TelegramBot : IJob
    {
        private readonly IBotUserService userService;
        private readonly IProfessionsService professionsService;
        private readonly IAccountService accountService;
        private readonly string botToken;
        private readonly TelegramBotClient client;
        public TelegramBot(IConfiguration config,IAccountService accountService, IBotUserService userService,IProfessionsService professionsService) 
        {
            this.accountService = accountService;
            this.professionsService = professionsService;
            this.userService = userService;
            botToken = config["TelegramBotToken"]!;
            client = new TelegramBotClient(botToken);
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var receiverOptions = new ReceiverOptions { AllowedUpdates = [] };
             client.StartReceiving(
               updateHandler: HandleUpdateAsync,
               pollingErrorHandler: HandleErrorAsync,
               receiverOptions: receiverOptions,
               cancellationToken: context.CancellationToken
            );
            await Task.Delay(Timeout.Infinite, context.CancellationToken);
        }
        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var message = update.Message;
            await CallbackQueryHandler( update, botClient, cancellationToken);
            if (message is not null)
            {
                switch (message.Type)
                {
                    case MessageType.Text:
                        await TextMessageHandler(message,botClient,cancellationToken);
                        break;
                    case MessageType.Contact:
                        await ContactMessageHandler(message, botClient, cancellationToken);
                        break;
                    default:
                        break;
                }
            }
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            DebugConsole.WriteLine(errorMessage, ConsoleColor.Red);
            return Task.CompletedTask;
        }
        
    }
}
