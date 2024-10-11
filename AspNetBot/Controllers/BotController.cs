using AspNetBot.Models;
using AspNetBot.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;

namespace AspNetBot.Controllers
{
    
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    [Route("api/[controller]")]
    public class BotController : ControllerBase
    {
        private readonly ITelegramBotClient botClient;
        public BotController(IConfiguration config) 
        {
            botClient = new TelegramBotClient(config["TelegramBotToken"]!);
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] MessageRequest message)
        {
            await botClient.SendTextMessageAsync(message.ChatId,message.Message);
            return Ok();
        }

    }
}
