using AspNetBot.Interafces;
using AspNetBot.Services;
using FluentValidation;

namespace AspNetBot.Extentions
{
    public static class ServicesExtentions
    {
        public static void AddServices(this IServiceCollection services) 
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IBotUserService, BotUserService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IProfessionsService, ProfessionsService>();
            services.AddScoped<IAccountService, AccountService>();
        } 
    }
}
