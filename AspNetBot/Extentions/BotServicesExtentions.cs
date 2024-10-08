using AspNetBot.Services;




namespace AspNetBot.Extentions
{
    public static class BotServicesExtentions
    {
        public static void AddChatBoot(this IServiceCollection services,IConfiguration config) 
        {
            services.AddSingleton(new TelegarmBotService(config));
            services.AddHostedService(sp => sp.GetRequiredService<TelegarmBotService>());
            
        }
    }
}
