using AspNetBot.Interafces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspNetBot.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class ProfessionController : ControllerBase
    {
        private readonly IProfessionsService professionsService;

        public ProfessionController(IProfessionsService professionsService)
        {
            this.professionsService = professionsService;
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetAll() => Ok(await professionsService.GetAllAsync(false));

        [HttpGet("get/{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id) => Ok(await professionsService.GetByIdAsync(id));
        

        [HttpPost("create/{name}")]
        public async Task<IActionResult> Create([FromRoute] string name) 
        {
            await professionsService.CreateAsync(name);
            return Ok();
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await professionsService.DeleteAsync(id);
            return Ok();
        }
        

    }
}
