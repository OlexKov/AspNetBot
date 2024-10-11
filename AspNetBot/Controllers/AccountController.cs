using AspNetBot.Helpers;
using AspNetBot.Interafces;
using AspNetBot.Models;
using AspNetBot.Models.Account;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetBot.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles ="Admin")]
    public class AccountController : ControllerBase
    {
        public readonly IAccountService accountService;
        
        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model) => Ok(await accountService.LoginAsync(model));
       
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromForm] UserCreationModel model) 
        {
            await accountService.SetAsync(model);
            return Ok();
        }
       
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromForm] UserCreationModel model)
        {
            await accountService.SetAsync(model);
            return Ok();
        }

        [HttpDelete("delete/{userId:int}")]
        public async Task<IActionResult> DeleteById([FromRoute]int userId)
        {
            await accountService.DeleteAsync(userId);
            return Ok();
        }
    }
}
