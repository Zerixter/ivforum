using IVForum.API.Auth;
using IVForum.API.Data;
using IVForum.API.Helpers;
using IVForum.API.Models;
using IVForum.API.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IVForum.API.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly DbHandler db;

        public AuthController(UserManager<UserModel> userManager, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions, DbHandler dbcontenxt)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            db = dbcontenxt;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CredentialsViewModel credentials)
        {
            List<object> Errors = new List<object>();
            if (credentials.UserName is null || credentials.Password is null)
            {
                if (credentials.UserName is null)
                {
                    Errors.Add(new { Missatge = "No s'ha introduit cap compte d'usuari." });
                }
                if (credentials.Password is null)
                {
                    Errors.Add(new { Missatge = "No s'ha introduit cap contrasenya." });
                }
                return BadRequest(Errors.ToArray());
            }

            var identity = await GetClaimsIdentity(credentials.UserName, credentials.Password);
            if (identity is null)
            {
                var userName = db.Users.Where(x => x.UserName == credentials.UserName).FirstOrDefault();
                if (userName is null)
                {
                    Errors.Add(new { Missatge = "El compte d'usuari introduit és incorrecte" });
                }
                else
                {
                    Errors.Add(new { Missatge = "La contrasenya introduida no és correcte" });
                }
                return BadRequest(Errors.ToArray());
            }

            var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, credentials.UserName, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
            return new OkObjectResult(jwt);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(userName);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            // check the credentials
            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id));
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }
    }
}
