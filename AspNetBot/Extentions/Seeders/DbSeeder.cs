using AspNetBot.Entities;
using AspNetBot.Helpers;
using AspNetBot.Interafces;
using Microsoft.AspNetCore.Identity;

namespace AspNetBot.Extentions.Seeders
{
    public static class DbSeeder
    {
        public static async Task SeedData(this WebApplication app,IConfiguration config)
        {
            using var scope = app.Services.CreateScope();
            var serviceProvider = scope.ServiceProvider;
                       
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            if (!roleManager.Roles.Any()) 
            {
                foreach (var role in Enum.GetNames<Roles>())
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }
            //============================================================================================
            var proffRepo = serviceProvider.GetRequiredService<IRepository<Profession>>();
            if (! await proffRepo.AnyAsync())
            {
                var professionsNames = config.GetRequiredSection("ProfessionsList").Get<List<string>>()!;
                var professions = professionsNames.AsParallel().Select(x=>new Profession() { Name = x});
                await proffRepo.AddRangeAsync(professions);
                await proffRepo.SaveAsync();
            }
        }
    }
}
