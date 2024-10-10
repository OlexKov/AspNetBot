using AspNetBot.Helpers;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using AspNetBot.Jobs;

namespace AspNetBot.Extentions.TBotExtensions
{
    public static class TextMessage
    {
        public static async Task TextMessageHandler(this TelegramBotJob bot, Message message, ITelegramBotClient botClient, CancellationToken cancellationToken)
        {
            var chatId = message.Chat.Id;
            DebugConsole.Write("Bot", ConsoleColor.Green);
            DebugConsole.WriteLine($":  User - {message.Chat.Username} send \"{message.Text}\"", ConsoleColor.Yellow);
            if (message.Text is not null)
            {
                var messageText = message.Text;
                switch (messageText)
                {
                    case "/start":
                        if (!await bot.IsUserExist(chatId))
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
        }
    }
}
