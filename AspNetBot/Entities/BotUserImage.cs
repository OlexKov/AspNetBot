namespace AspNetBot.Entities
{
    public class BotUserImage
    {
        public int Id { get; set; } 
        public string Name { get; set; } = string.Empty;
        public string BotUserId { get; set; }
        public BotUser BotUser { get; set; }
    }
}
