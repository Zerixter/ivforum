using IVForum.API.Auth;
using IVForum.API.Data;
using IVForum.API.Helpers;
using IVForum.API.Models;
using IVForum.API.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IVForum.API.Controllers
{
    [Produces("application/json")]
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly UserManager<UserModel> userManager;
        private readonly IJwtFactory jwtFactory;
        private readonly JwtIssuerOptions jwtOptions;
        private readonly DbHandler db;

        public AccountController(UserManager<UserModel> _userManager, IJwtFactory _jwtFactory, IOptions<JwtIssuerOptions> _jwtOptions, DbHandler _db)
        {
            userManager = _userManager;
            jwtFactory = _jwtFactory;
            jwtOptions = _jwtOptions.Value;
            db = _db;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            List<object> Errors = new List<object>();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string regexMail = @"^[a-z0-9._%+-]+@[a-z0-9.-]+[^\.]\.[a-z]{2,3}$";
            Regex regex = new Regex(regexMail);

            if (!regex.Match(model.Email).Success)
            {
                Errors.Add(new { Message = "Mail incorrecto" });
                return BadRequest(Errors);
            }

            string regexPassword = "^(?=.*[A-Z])(?=.*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{8,}";
            regex = new Regex(regexPassword);
            if (!regex.Match(model.Password).Success)
            {
                Errors.Add(new { Message = "La contrasenya debe ser mejor..." });
                return BadREquest(Errors);
            }

            UserModel userModel = new UserModel
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                UserName = model.Email
            };

            User user = new User
            {
                Id = Guid.NewGuid(),
                Identity = userModel
            };

            var result = await userManager.CreateAsync(userModel, model.Password);

            await db.DbUsers.AddAsync(user);
            await db.SaveChangesAsync();

            var jsonResult = new
            {
                Code = result.Succeeded,
                Status = "aaa"
            };

            return new JsonResult(jsonResult);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]CredentialsViewModel credentials)
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

            var jwt = await Tokens.GenerateJwt(identity, jwtFactory, credentials.UserName, jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
            return new OkObjectResult(jwt);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            // get the user to verifty
            var userToVerify = await userManager.FindByNameAsync(userName);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            // check the credentials
            if (await userManager.CheckPasswordAsync(userToVerify, password))
            {
                return await Task.FromResult(jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id));
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }
    }
}