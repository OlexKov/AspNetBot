using AspNetBot.Interafces;
using AspNetBot.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetBot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly IBotUserService botUserService;

        public UserController(IBotUserService botUserService)
        {
            this.botUserService = botUserService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetAll() => Ok(await botUserService.GetAllAsync());

        [HttpGet("get/{page:int}/{size:int}")]
        public  IActionResult GetPagination([FromRoute] int size,int page) => Ok( botUserService.GetPagination(page,size));

        [HttpGet("get/profid/{profId:int}")]
        public async Task<IActionResult> GetByProfessionId([FromRoute] int profId) => Ok(await botUserService.GetAllByProfessionAsync(profId));

        [HttpGet("get/profname/{proffName}")]
        public async Task<IActionResult> GetByProfessionName([FromRoute] string profName) => Ok(await botUserService.GetAllByProfessionAsync(profName));

        [HttpGet("get/chat/{chatId:long}")]
        public async Task<IActionResult> GetByChatId([FromRoute] long chatId) => Ok(await botUserService.GetByChatIdAsync(chatId));
        
        [HttpGet("get/{userId:int}")]
        public async Task<IActionResult> GetById([FromRoute] int userId) => Ok(await botUserService.GetByIdAsync(userId));

        [HttpPost("set/{profId:int}/{chatId:long}")]
        public async Task<IActionResult> SetUserProfession([FromRoute] int profId, long chatId) => Ok(await botUserService.SetUserProfessionAsync(chatId,profId));
              
        [HttpDelete("delete/{chatId:int}")]
        public async Task<IActionResult> DeleteById([FromRoute] int chatId)
        {
            await botUserService.DeleteAsync(chatId);
            return Ok();
        }
    }
}
