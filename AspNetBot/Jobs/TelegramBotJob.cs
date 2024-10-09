using AspNetBot.Helpers;
using AspNetBot.Interafces;
using AspNetBot.Models;
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
        private readonly IBotUserService userService;
        private readonly string botToken;
        public TelegramBotJob(IConfiguration config, IBotUserService userService) 
        {
            this.userService = userService;
            botToken = config["TelegramBotToken"]!;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            var receiverOptions = new ReceiverOptions { AllowedUpdates = [] };
            new TelegramBotClient(botToken).StartReceiving(
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
            if (message is not null)
            {
                switch (message.Type)
                {
                    case MessageType.Text:
                        DebugConsole.Write("Bot", ConsoleColor.Green);
                        DebugConsole.WriteLine($":  User - {message.Chat.Username} send \"{message.Text}\"", ConsoleColor.Yellow);
                        if (message.Text is not null)
                        {
                            var messageText = message.Text;
                            var chatId = message.Chat.Id;
                            switch (messageText)
                            {
                                case "/start":
                                    if (!await isUserExist(chatId))
                                    {
                                        var shareContactButton = new KeyboardButton("Поділитися номером телефону") { RequestContact = true };

                                        var replyKeyboardMarkup = new ReplyKeyboardMarkup(new[] { new[] { shareContactButton } })
                                        {
                                            ResizeKeyboard = true,
                                            OneTimeKeyboard = true
                                        };
                                        await botClient.SendTextMessageAsync(
                                            chatId,
                                            "Натисніть кнопку, щоб поділитися своїм номером телефону:",
                                            replyMarkup: replyKeyboardMarkup,
                                            cancellationToken: cancellationToken);
                                    }
                                    else
                                    {
                                        await botClient.SendTextMessageAsync(
                                            chatId,
                                            $"Вітаємо: {message.Chat.FirstName} {message.Chat.LastName} !!!",
                                            cancellationToken: cancellationToken);
                                    }
                                    break;
                                default:
                                    await botClient.SendTextMessageAsync(
                                        chatId,
                                        $"Ви написали: {messageText}",
                                        cancellationToken: cancellationToken);
                                    break;
                            }
                        }
                        break;
                    case MessageType.Contact:
                        if (message.Contact is not null)
                        {
                            var chatId = message.Chat.Id;
                            if (!await isUserExist(chatId))
                            {
                                var contact = message.Contact;
                                string phoneNumber = contact.PhoneNumber;
                                string? userPhotoUrl = null;
                                var userPhotos = await botClient.GetUserProfilePhotosAsync(
                                    chatId,
                                    cancellationToken: cancellationToken);
                                if (userPhotos.TotalCount > 0)
                                {
                                    var largestPhoto = userPhotos.Photos[0][^1];
                                    var file = await botClient.GetFileAsync(largestPhoto.FileId);
                                    userPhotoUrl = $"https://api.telegram.org/file/bot{botToken}/{file.FilePath}";
                                }
                                var newBotUser = new BotUserCreationModel()
                                {
                                    FirstName = contact.FirstName,
                                    LastName = contact.LastName,
                                    UserName = message.Chat.Username,
                                    UserId = chatId,
                                    ImageUrl = userPhotoUrl,
                                    PhoneNumber = phoneNumber
                                };
                                await userService.set(newBotUser);
                            }
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
        public async Task<bool> isUserExist(long id) => await userService.getByChatId(id) != null;
    }
}
