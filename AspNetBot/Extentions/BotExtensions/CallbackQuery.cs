using AspNetBot.Helpers;
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
                if (int.TryParse(callbackQuery.Data, out int professionId) && chatId != 0)
                {
                    if (callbackQuery.Message != null)
                    {
                        int messageId = callbackQuery.Message.MessageId;
                        try
                        {
                            var user = await bot.UserService.SetUserProfessionAsync(chatId, professionId);
                            await botClient.EditMessageTextAsync(chatId: chatId, messageId: messageId, $"Дякую,ваша професія: {user.ProfessionName}", replyMarkup: null, cancellationToken: cancellationToken);
                        }
                        catch (Exception ex)
                        {
                            DebugConsole.WriteLine(ex.Message,ConsoleColor.Red);
                        }
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
