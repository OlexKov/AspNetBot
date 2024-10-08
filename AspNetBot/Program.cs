using AspNetBot.Extentions;
using AspNetBot.Extentions.Seeders;
using AspNetBot.Services;
using AspNetBot.Новая_папка;


var builder = WebApplication.CreateBuilder(args);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var connStr = builder.Configuration.GetConnectionString("DefaultConnection")!;

builder.Services.AddTelegramBotDbContext(connStr);

builder.Services.AddJwtServices(builder.Configuration);

builder.Services.AddServices();

builder.Services.AddChatBoot(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddJobs();

var app = builder.Build();

app.UseCors("AllowOrigins");

app.SeedData().Wait();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
