using Telegram.Bot.Types.ReplyMarkups;

namespace AspNetBot.Telegram
{
    public partial class TelegramBot
    {
        public async Task<bool> IsUserExist(long id) => await userService.GetByChatIdAsync(id) != null;
        public InlineKeyboardButton[][] CreateInlineButtons(Dictionary<string, string> data, int colums)
        {
            return data.AsParallel().Select(x => InlineKeyboardButton.WithCallbackData(text: x.Key, callbackData: x.Value))
                       .Select((button, index) => new { Button = button, Index = index })
                       .GroupBy(x => x.Index / colums)
                       .Select(g => g.Select(x => x.Button).ToArray())
                       .ToArray();
        }
    }
}
