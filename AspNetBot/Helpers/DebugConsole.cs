using Telegram.Bot.Types;

namespace AspNetBot.Helpers
{
    public static class DebugConsole
    {
        public static void WriteLine(string message, ConsoleColor color)
        {
            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = defaultColor;
        }

        public static void Write(string message, ConsoleColor color)
        {
            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(message);
            Console.ForegroundColor = defaultColor;
        }
    }
}
