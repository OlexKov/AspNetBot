using AspNetBot.Entities;

namespace AspNetBot.Models
{
    public class UserCreationModel
    {    
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public long? ChatId { get; set; }
        public int? ProfessionId { get; set; }
        public string? PhoneNumber { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string? ImageUrl { get; set; } 
        public string? UserName { get; set; }
    }
}
