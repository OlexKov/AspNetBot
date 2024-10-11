namespace AspNetBot.Models
{
    public class MessageRequest
    {
        public string Message { get; set; } = string.Empty;
        public long ChatId { get; set; }
    }
}
