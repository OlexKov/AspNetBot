﻿using Microsoft.AspNetCore.Identity;

namespace AspNetBot.Entities
{
    public class BotUser : IdentityUser<int>
    {
        public string? FirstName { get; set; } 
        public string? LastName { get; set; }
        public long? ChatId { get; set; }
        public int? ProfessionId { get; set; }
        public Profession? Profession { get; set; }
        public string? Image { get; set; }
    }
}
