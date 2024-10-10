using AspNetBot.DataBaseContext;
using AspNetBot.Entities;
using AspNetBot.Interafces;
using AspNetBot.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AspNetBot.Extentions
{
    public static class DbContextServiceExstentions
    {
        public static void AddTelegramBotDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<BotDbContext>(opts =>
                opts.UseNpgsql(connectionString));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddIdentity<BotUser, IdentityRole<int>>(options =>
            {
                options.Stores.MaxLengthForKeys = 128;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
               .AddDefaultTokenProviders()
               .AddEntityFrameworkStores<BotDbContext>();
        }
    }
}
