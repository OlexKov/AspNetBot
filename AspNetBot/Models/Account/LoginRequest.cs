namespace AspNetBot.Models.Account
{
    public class LoginRequest
    {
        /// <summary>
        /// Пошта користувача
        /// </summary>
        /// <example>admin@gmail.com</example>
        public string Email { get; set; } = string.Empty;
        /// <summary>
        /// Пароль користувача
        /// </summary>
        /// <example>Admin_1</example>
        public string Password { get; set; } = string.Empty;
    }
}
