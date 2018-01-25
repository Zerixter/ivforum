using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;

using WebAPI.Models;

namespace WebAPI.Controllers
{
	[Produces("application/json")]
    [Route("api/token")]
    public class TokenController : Controller
    {
        private IConfiguration Config;
        private ApplicationDbContext Context;

        public TokenController(IConfiguration config, ApplicationDbContext context)
        {
            Context = context;
            Config = config;
        }

        [HttpPost]
        public IActionResult CreateToken([FromBody]User login)
        {
            IActionResult response = Unauthorized();

            var user = Authenticate(login);

            if (user != null)
            {
                var tokenString = BuildToken();
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        private string BuildToken()
        {
			SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["Jwt:Key"]));
			SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			JwtSecurityToken token = 
				new JwtSecurityToken(
					Config["Jwt:Issuer"],
					Config["Jwt:Issuer"],
					expires: DateTime.Now.AddMinutes(30),
					signingCredentials: creds
				);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User Authenticate(User login)
        {
			return null;
        }
    }
}