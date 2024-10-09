using AspNetBot.Entities;

namespace AspNetBot.Models
{
    public class BotUserCreationModel
    {    
        public string Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public long? UserId { get; set; }
        public int? ProfessionId { get; set; }
        public string? PhoneNumber { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? ImageUrl { get; set; } 
        public string? UserName { get; set; }
    }
}
