using AspNetBot.Entities;
using AspNetBot.Helpers;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;


namespace AspNetBot.Services
{
    public class TelegarmBotService(IConfiguration config) : BackgroundService
    {
        private readonly ITelegramBotClient client = new TelegramBotClient(config["TelegramBotToken"]!);

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = []
            };

            client.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandleErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: stoppingToken
            );

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var message = update.Message;
            if (message is not null)
            {
                switch (message.Type)
                {
                    case MessageType.Text:
                        DebugConsole.Write("Bot", ConsoleColor.Green);
                        DebugConsole.WriteLine($": User - {message.Chat.Username} send \"{message.Text}\"", ConsoleColor.Yellow);
                        if (message.Text is not null)
                        {
                            var messageText = message.Text;
                            var chatId = message.Chat.Id;
                            await client.SendTextMessageAsync(chatId, $"Ви написали: {messageText}", cancellationToken: cancellationToken);
                        }
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

        public async Task SendMessages(IEnumerable<BotUser> users)
        {
            DebugConsole.WriteLine("Messsages sended", ConsoleColor.Blue);
        }
    }
}
