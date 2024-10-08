using Microsoft.AspNetCore.Identity;

namespace AspNetBot.Entities
{
    public class BotUser : IdentityUser
    {
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string ChatUserName { get; set; } = string.Empty;
        public long UserId { get; set; }
        public int ProfessionId { get; set; }
        public Profession Profession { get; set; }
        public ICollection<BotUserImage> Images { get; set; } = new HashSet<BotUserImage>();
    }
}
