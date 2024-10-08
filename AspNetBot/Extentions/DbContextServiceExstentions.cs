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
            services.AddIdentity<BotUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
            })
               .AddDefaultTokenProviders()
               .AddEntityFrameworkStores<BotDbContext>();
        }
    }
}
