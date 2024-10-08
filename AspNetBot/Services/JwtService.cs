using AspNetBot.Entities;
using AspNetBot.Exceptions;
using AspNetBot.Helpers;
using AspNetBot.Interafces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Telegram.Bot.Types;

namespace AspNetBot.Services
{
    public class JwtService(IConfiguration configuration,UserManager<BotUser> userManager) : IJwtService
    {
        private readonly JwtOptions jwtOpts = configuration.GetSection(nameof(JwtOptions)).Get<JwtOptions>()
                ?? throw new HttpException("Invalid JWT setting", HttpStatusCode.InternalServerError);
        private readonly UserManager<BotUser> userManager = userManager;

        public async Task<IEnumerable<Claim>> GetClaimsAsync(BotUser user)
        {
            var rolesJson = JsonConvert.SerializeObject(await userManager.GetRolesAsync(user));
            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.Id),
                new (ClaimTypes.Surname, user.LastName??""),
                new (ClaimTypes.Name, user.FirstName??""),
                new (ClaimTypes.HomePhone, user.PhoneNumber??""),
                new (ClaimTypes.Role,rolesJson)
            };
            return claims;
        }

        private SigningCredentials getCredentials(JwtOptions options)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Key));
            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        }

        public async Task<string> CreateTokenAsync(BotUser user)
        {
            var claims = await GetClaimsAsync(user);
            var time = DateTime.UtcNow.AddHours(jwtOpts.Lifetime);
            var credentials = getCredentials(jwtOpts);
            var token = new JwtSecurityToken(
                issuer: jwtOpts.Issuer,
                claims: claims,
                expires: time,
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
