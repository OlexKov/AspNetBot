namespace AspNetBot.Helpers
{
    public class JwtOptions
    {
        public string Key { get; set; } = string.Empty;
        public int Lifetime { get; set; }
        public string Issuer { get; set; } = string.Empty;
    }
}
