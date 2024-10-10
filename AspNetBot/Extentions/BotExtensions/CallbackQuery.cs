using AspNetBot.Jobs;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace AspNetBot.Extentions.TBotExtensions
{
    public static class CallbackQuery
    {
        public static async Task CallbackQueryHandler(this TelegramBotJob bot, Update update, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            var callbackQuery = update.CallbackQuery;
            if (callbackQuery != null)
            {
                var chatId = callbackQuery.From.Id;
                if (int.TryParse(callbackQuery.Data, out int professionId))
                {
                    if (callbackQuery.Message != null)
                    {
                        int messageId = callbackQuery.Message.MessageId;
                        var user = await bot.UserService.setUserProfession(chatId, professionId);
                        await botClient.EditMessageReplyMarkupAsync(chatId: chatId, messageId: messageId, replyMarkup: null, cancellationToken: cancellationToken);
                        await botClient.EditMessageTextAsync(chatId: chatId, messageId: messageId, $"Ваша професія - {user.ProfessionName}", cancellationToken: cancellationToken);
                    }
                }
                else
                {
                    switch (callbackQuery.Data)
                    {
                        default:
                            break;
                    };
                }
            }
        }
    }
}
