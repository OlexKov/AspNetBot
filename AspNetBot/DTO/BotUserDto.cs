using AspNetBot.Entities;

namespace AspNetBot.DTO
{
    public class BotUserDto
    {
        public string? PhoneNumber { get; set; } = string.Empty;
        public int Id { get; set; }
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public long ChatId { get; set; }
        public int ProfessionId { get; set; }
        public string? ProfessionName { get; set; } = string.Empty;
        public string? Image { get; set; } = string.Empty;
    }
}
