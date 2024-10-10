using AspNetBot.Extentions.TBotExtensions;
using AspNetBot.Helpers;
using AspNetBot.Interafces;
using AspNetBot.Services;
using Quartz;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace AspNetBot.Jobs
{
    public class TelegramBotJob : IJob
    {
        public IBotUserService UserService { get; private set; }
        public IProfessionsService ProfessionsService { get; private set; }
        public IAccountService AccountService { get; private set; }
        public string BotToken { get; private set; }
        private readonly TelegramBotClient client;
        public TelegramBotJob(IConfiguration config,IAccountService accountService, IBotUserService userService,IProfessionsService professionsService) 
        {
            AccountService = accountService;
            ProfessionsService = professionsService;
            UserService = userService;
            BotToken = config["TelegramBotToken"]!;
            client = new TelegramBotClient(BotToken);
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
            await this.CallbackQueryHandler( update, botClient, cancellationToken);
            if (message is not null)
            {
                switch (message.Type)
                {
                    case MessageType.Text:
                        await  this.TextMessageHandler(message,botClient,cancellationToken);
                        break;
                    case MessageType.Contact:
                        await this.ContactMessageHandler(message, botClient, cancellationToken);
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

        public async Task<bool> IsUserExist(long id) => await UserService.GetByChatIdAsync(id) != null;
        public InlineKeyboardButton[][] CreateInlineButtons(Dictionary<string, string> data,int colums)
        { 
            return data.AsParallel().Select(x=> InlineKeyboardButton.WithCallbackData(text: x.Key, callbackData: x.Value))
                       .Select((button, index) => new { Button = button, Index = index })
                       .GroupBy(x => x.Index / colums)
                       .Select(g => g.Select(x => x.Button).ToArray())
                       .ToArray();
        }
    }
}
