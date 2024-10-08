using AspNetBot.Helpers;
using Microsoft.AspNetCore.Identity;

namespace AspNetBot.Extentions.Seeders
{
    public static class DbSeeder
    {
        public static async Task SeedData(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
                       
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            if(!roleManager.Roles.Any())
            foreach (var role in Enum.GetNames<Roles>())
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
