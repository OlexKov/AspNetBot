using Telegram.Bot.Types;
using Telegram.Bot;
using AspNetBot.Models;
using Telegram.Bot.Types.ReplyMarkups;
using AspNetBot.Jobs;

namespace AspNetBot.Extentions.TBotExtensions
{
    public static class ContactMessage
    {
        public static async Task ContactMessageHandler(this TelegramBotJob bot, Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            if (message.Contact is not null)
            {
                var chatId = message.Chat.Id;
                if (!await bot.IsUserExist(chatId))
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
                        userPhotoUrl = $"https://api.telegram.org/file/bot{bot.BotToken}/{file.FilePath}";
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
                    await bot.UserService.set(newBotUser);
                    var professions = await bot.ProfessionsService.GetAll(false);

                    //await botClient.EditMessageReplyMarkupAsync(
                    //    chatId: chatId, 
                    //    messageId: message.MessageId, 
                    //    replyMarkup: new  ReplyKeyboardRemove(), 
                    //    cancellationToken: cancellationToken);
                    var inlineButtons = professions
                        .Select(x => InlineKeyboardButton.WithCallbackData(text: x.Name, callbackData: x.Id.ToString()))
                        .Select((button, index) => new { Button = button, Index = index })
                        .GroupBy(x => x.Index / 3)
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
        }
    }
}
