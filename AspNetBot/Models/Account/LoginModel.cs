namespace AspNetBot.Models.Account
{
    public class LoginModel
    {
        /// <summary>
        /// Пошта користувача
        /// </summary>
        /// <example>admin@gmail.com</example>
        public string? Email { get; set; }
        /// <summary>
        /// Пароль користувача
        /// </summary>
        /// <example>Admin_1</example>
        public string? Password { get; set; }
    }
}
