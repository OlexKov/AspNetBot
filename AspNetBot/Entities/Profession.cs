namespace AspNetBot.Entities
{
    public class Profession
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<BotUser> Users { get; set; } = new HashSet<BotUser>();
    }
}
