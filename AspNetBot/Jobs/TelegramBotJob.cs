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
        private readonly IProfessionsService professionsService;
        private readonly string botToken;
        private readonly TelegramBotClient client;
        public TelegramBotJob(IConfiguration config, IBotUserService userService,IProfessionsService professionsService) 
        {
            
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
            var callbackQuery = update.CallbackQuery;
            if (callbackQuery != null)
            {
                var chatId = callbackQuery.From.Id;
                if (int.TryParse(callbackQuery.Data, out int professionId))
                {
                    if (callbackQuery.Message != null) 
                    {
                        int messageId = callbackQuery.Message.MessageId;
                        var user = await userService.setUserProfession(chatId, professionId);
                        await botClient.EditMessageReplyMarkupAsync(
                                           chatId: chatId,
                                           messageId: messageId,
                                           replyMarkup: null);
                        await botClient.EditMessageTextAsync(chatId: chatId,
                                            messageId: messageId, $"Ваша професія - {user.ProfessionName}");
                    }
                }
                else
                {
                     switch(callbackQuery.Data)
                    {
                        default:
                            break;
                    };
                }
            }

            if (message is not null)
            {
                var chatId = message.Chat.Id;
               
                switch (message.Type)
                {
                    case MessageType.Text:
                        DebugConsole.Write("Bot", ConsoleColor.Green);
                        DebugConsole.WriteLine($":  User - {message.Chat.Username} send \"{message.Text}\"", ConsoleColor.Yellow);
                        if (message.Text is not null)
                        {
                            var messageText = message.Text;
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
                                    var fileId = userPhotos.Photos[0][^1].FileId;
                                    var file = await botClient.GetFileAsync(fileId, cancellationToken: cancellationToken);
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
                                var professions = await professionsService.GetAll(false);
                                var inlineButtons = professions
                                    .Select(x => InlineKeyboardButton.WithCallbackData(text: x.Name, callbackData: x.Id.ToString()))
                                    .Select((button, index) => new { Button = button, Index = index })
                                    .GroupBy(x => x.Index / 2)
                                    .Select(g => g.Select(x => x.Button).ToArray())
                                    .ToArray();
                                var inlineKeyboard = new InlineKeyboardMarkup(inlineButtons);
                                await botClient.SendTextMessageAsync(
                                            chatId,
                                            "Оберіть професію",
                                            replyMarkup: inlineKeyboard,
                                            cancellationToken: cancellationToken);
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
