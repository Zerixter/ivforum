using IVForum.API.Auth;
using IVForum.API.Data;
using IVForum.API.Helpers;
using IVForum.API.Models;
using IVForum.API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IVForum.API.Controllers
{
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly UserManager<UserModel> userManager;
        private readonly IJwtFactory jwtFactory;
        private readonly JwtIssuerOptions jwtOptions;
        private readonly DbHandler db;
        private readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };
        private readonly ClaimsPrincipal claimsPrincipal;

        public AccountController(UserManager<UserModel> _userManager, IJwtFactory _jwtFactory, IOptions<JwtIssuerOptions> _jwtOptions, DbHandler _db, IHttpContextAccessor httpContextAccessor)
        {
            userManager = _userManager;
            jwtFactory = _jwtFactory;
            jwtOptions = _jwtOptions.Value;
            db = _db;
            claimsPrincipal = httpContextAccessor.HttpContext.User;
        }

        [HttpGet("get")]
        public IActionResult Get()
        {
            User user = null;

            try
            {
                var userId = claimsPrincipal.Claims.Single(c => c.Type == "id");
                user = db.DbUsers.SingleAsync(c => c.IdentityId == userId.Value).GetAwaiter().GetResult();
            } catch (Exception)
            {
                var Message = new
                {
                    Message = "S'ha de loguejar por poder visualitzar les dades d'usuari."
                };
                return BadRequest(Message);
            }

            return new JsonResult(user);
        }

        [HttpGet("get/{userid}")]
        public IActionResult Get(string userid)
        {
            User user = null;

            try
            {
                user = db.DbUsers.SingleAsync(c => c.IdentityId == userid).GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                var Message = new
                {
                    Message = "S'ha de loguejar por poder visualitzar les dades d'usuari."
                };
                return BadRequest(Message);
            }

            return new JsonResult(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            List<object> Errors = new List<object>();

            try
            {
                string regexPassword = "^(?=.*[A-Z])(?=.*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{8,}";
                Regex regex = new Regex(regexPassword);
                if (!regex.IsMatch(model.Password))
                {
                    Errors.Add(new { Message = "La contrasenya introduida no és correcte: El format ha de ser el següent, ha de tenir mínim un numero, una lletra minúscula i una majuscula i ha de tenir una longitura de mínim 8 carácters" });
                }
            } catch (Exception)
            {
                Errors.Add(new { Message = "No s'ha introduit cap contrasenya." });
            }

            try
            {
                string regexMail = @"^[a-z0-9._%+-]+@[a-z0-9.-]+[^\.]\.[a-z]{2,3}$";
                Regex regex = new Regex(regexMail);
                if (!regex.IsMatch(model.Email))
                {
                    Errors.Add(new { Messatge = "El correu electrònic introduit no és correcte." });
                }
            } catch (Exception)
            {
                Errors.Add(new { Message = "No s'ha introduit cap correu electrònic." });
            }

            if (model.Name is null)
            {
                Errors.Add(new { Message = "El camp del nom s'ha deixat buit, s'ha de posar un nom." });
            }

            if (model.Surname is null)
            {
                Errors.Add(new { Message = "El camp del cognom s'ha deixat buit, s'ha de posar un cognom." });
            }

            if (Errors.Count >= 1)
            {
                return BadRequest(Errors);
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

            db.DbUsers.Add(user);
            db.SaveChanges();

            var identity = await GetClaimsIdentity(model.Email, model.Password);

            var jwt = await Tokens.GenerateJwt(identity, jwtFactory, model.Email, jwtOptions, jsonSerializerSettings);
            return new OkObjectResult(jwt);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]CredentialsViewModel credentials)
        {
            List<object> Errors = new List<object>();
            if (credentials.Email is null || credentials.Password is null)
            {
                if (credentials.Email is null)
                {
                    Errors.Add(new { Missatge = "No s'ha introduit cap compte d'usuari." });
                }
                if (credentials.Password is null)
                {
                    Errors.Add(new { Missatge = "No s'ha introduit cap contrasenya." });
                }
                return BadRequest(Errors);
            }

            var identity = await GetClaimsIdentity(credentials.Email, credentials.Password);
            if (identity is null)
            {
                var userName = db.Users.Where(x => x.UserName == credentials.Email).FirstOrDefault();
                if (userName is null)
                {
                    Errors.Add(new { Missatge = "El compte d'usuari introduit és incorrecte" });
                } else
                {
                    Errors.Add(new { Missatge = "La contrasenya introduida no és correcte" });
                }
                return BadRequest(Errors.ToArray());
            }

            var jwt = await Tokens.GenerateJwt(identity, jwtFactory, credentials.Email, jwtOptions, jsonSerializerSettings);
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