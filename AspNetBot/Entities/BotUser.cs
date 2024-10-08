using Microsoft.AspNetCore.Identity;

namespace AspNetBot.Entities
{
    public class BotUser:IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}
