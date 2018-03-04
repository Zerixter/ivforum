using IVForum.API.Auth;
using IVForum.API.Classes;
using IVForum.API.Data;
using IVForum.API.Helpers;
using IVForum.API.Models;
using IVForum.API.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    [EnableCors("all")]
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly UserManager<UserModel> userManager;
        private readonly IJwtFactory jwtFactory;
        private readonly JwtIssuerOptions jwtOptions;
        private readonly DbHandler db;
        private readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings { Formatting = Formatting.Indented };
        private readonly ClaimsPrincipal claimsPrincipal;
        private readonly UserGetter userGetter;

        public AccountController(UserManager<UserModel> _userManager, IJwtFactory _jwtFactory, IOptions<JwtIssuerOptions> _jwtOptions, DbHandler _db, IHttpContextAccessor httpContextAccessor)
        {
            userManager = _userManager;
            jwtFactory = _jwtFactory;
            jwtOptions = _jwtOptions.Value;
            db = _db;
            claimsPrincipal = httpContextAccessor.HttpContext.User;
            userGetter = new UserGetter(db, httpContextAccessor);
        }

        [Authorize(Policy = "ApiUser")]
        [HttpGet("get")]
        public IActionResult Get()
        {
            User user = userGetter.GetUser();

            UserViewModel model = new UserViewModel
            {
                Id = user.Id.ToString(),
                Avatar = user.Avatar,
                Name = user.Identity.Name,
                Surname = user.Identity.Surname,
                Description = user.Description,
                Email = user.Identity.Email,
                FacebookUrl = user.FacebookUrl,
                RepositoryUrl = user.RepositoryUrl,
                TwitterUrl = user.TwitterUrl,
                WebsiteUrl = user.WebsiteUrl
            };

            if (user is null)
            {
                return BadRequest(Message.GetMessage("No hi ha cap usuari connectat per poder visualtizar les dades."));
            }
            return new JsonResult(model);
        }

        [Authorize(Policy = "ApiUser")]
        [HttpGet("get/{userid}")]
        public IActionResult Get(string userid)
        {
            User user = userGetter.GetUser(userid);

            UserViewModel model = new UserViewModel
            {
                Id = user.Id.ToString(),
                Name = user.Identity.Name,
                Surname = user.Identity.Surname,
                Avatar = user.Avatar,
                Description = user.Description,
                Email = user.Identity.Email,
                FacebookUrl = user.FacebookUrl,
                RepositoryUrl = user.RepositoryUrl,
                TwitterUrl = user.TwitterUrl,
                WebsiteUrl = user.WebsiteUrl
            };

            if (user is null)
            {
                return BadRequest(Message.GetMessage("No existeix cap usuari amb aquesta id en la base de dades."));
            }
            return new JsonResult(model);
        }

        [Authorize(Policy = "ApiUser")]
        [HttpGet("get/forum/{id_forum}")]
        public IActionResult GetForum(string id_forum)
        {
            User user = userGetter.GetUser();
            if (user is null)
            {
                return BadRequest(Message.GetMessage("No hi ha cap usuari connectat."));
            }

            Wallet wallet = db.Wallets.Where(x => x.User.Id == user.Id && x.Forum.Id.ToString() == id_forum).Include(x => x.User).Include(x => x.Forum).FirstOrDefault();
            if (wallet is null)
            {
                return BadRequest();
            }
            return new JsonResult(wallet);
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
                    Errors.Add(Message.GetMessage("La contrasenya introduida no és correcte: El format ha de ser el següent, ha de tenir mínim un numero, una lletra minúscula i una majuscula i ha de tenir una longitura de mínim 8 carácters"));
                } else
                {
                    var UserCheck = db.Users.Where(x => x.UserName == model.Email).FirstOrDefault();
                    if (UserCheck != null)
                    {
                        Errors.Add(Message.GetMessage("Un usuari amb amb aquest correu electrònic ja existeix."));
                    }
                }
            } catch (Exception)
            {
                Errors.Add(Message.GetMessage("No s'ha introduit cap contrasenya."));
            }

            try
            {
                string regexMail = @"^[a-z0-9._%+-]+@[a-z0-9.-]+[^\.]\.[a-z]{2,3}$";
                Regex regex = new Regex(regexMail);
                if (!regex.IsMatch(model.Email))
                {
                    Errors.Add(Message.GetMessage("El correu electrònic introduit no és correcte."));
                }
            } catch (Exception)
            {
                Errors.Add(Message.GetMessage("No s'ha introduit cap correu electrònic."));
            }

            if (model.Name is null)
            {
                Errors.Add(Message.GetMessage("El camp del nom s'ha deixat buit, s'ha de posar un nom."));
            }

            if (model.Surname is null)
            {
                Errors.Add(Message.GetMessage("El camp del cognom s'ha deixat buit, s'ha de posar un cognom."));
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
                    Errors.Add(Message.GetMessage("No s'ha introduit cap compte d'usuari."));
                }
                if (credentials.Password is null)
                {
                    Errors.Add(Message.GetMessage("No s'ha introduit cap contrasenya."));
                }
                return BadRequest(Errors);
            }

            var identity = await GetClaimsIdentity(credentials.Email, credentials.Password);
            if (identity is null)
            {
                var userName = db.Users.Where(x => x.UserName == credentials.Email).FirstOrDefault();
                if (userName is null)
                {
                    Errors.Add(Message.GetMessage("El compte d'usuari introduit és incorrecte"));
                } else
                {
                    Errors.Add(Message.GetMessage("La contrasenya introduida no és correcte"));
                }
                return BadRequest(Errors);
            }

            var jwt = await Tokens.GenerateJwt(identity, jwtFactory, credentials.Email, jwtOptions, jsonSerializerSettings);
            return new OkObjectResult(jwt);
        }

        [Authorize(Policy = "ApiUser")]
        [HttpPost("update")]
        public IActionResult Update([FromBody]UserViewModel model)
        {
            User UserToEdit = userGetter.GetUser();
            if (UserToEdit is null)
            {
                return BadRequest(Message.GetMessage("Per poder modificar les dades d'un usuari s'ha de loguejar primer."));
            }

            UserToEdit = UpdateUser(UserToEdit, model);

            db.DbUsers.Update(UserToEdit);
            db.SaveChanges();

            return new JsonResult(Message.GetMessage("S'ha modificat les dades del usuari correctament."));
        }

        [Authorize(Policy = "ApiUser")]
        [HttpGet("delete")]
        public async Task<IActionResult> Delete()
        {
            try
            {
                var userId = claimsPrincipal.Claims.Single(c => c.Type == "id");
                var ASPUser = await db.Users.SingleAsync(c => c.Id == userId.Value);
                var DBUser = await db.DbUsers.SingleAsync(c => c.IdentityId == userId.Value);

                if (ASPUser != null && DBUser != null)
                {
                    db.Remove(DBUser);
                    db.Remove(ASPUser);
                    db.SaveChanges();

                    return new JsonResult(Message.GetMessage("S'ha esborrat el usuari correctament."));
                }
                return BadRequest(Message.GetMessage("S'ha produit un error al intentar esborrar el usuari (No hi ha cap usuari loguejat)."));
            } catch (Exception)
            {
                return BadRequest(Message.GetMessage("S'ha produit un error al intentar esborrar el usuari (No hi ha cap usuari loguejat)."));
            }
        }

        [HttpPost("subscribe")]
        public IActionResult Subscribe([FromBody]Forum forum)
        {
            Forum ForumToSearch = db.Forums.Where(x => x.Id == forum.Id).SingleOrDefault();
            if (ForumToSearch is null)
            {
                return BadRequest(Message.GetMessage("No existeix cap forum amb aquesta id en la base de dades."));
            }

            User user = userGetter.GetUser();
            if (user is null)
            {
                return BadRequest(Message.GetMessage("No hi ha cap usuari connectat ara mateix. Connecta't per poder subscriure a un forum"));
            }

            Wallet wallet = new Wallet
            {
                Id = Guid.NewGuid(),
                Forum = ForumToSearch,
                User = user
            };

            db.Wallets.Add(wallet);
            db.SaveChanges();

            AddBillOptions(ForumToSearch, wallet);
            return new JsonResult(Message.GetMessage("El usuari s'ha subscrit exitosament a aquest forum."));
        }

        [Authorize(Policy = "ApiUser")]
        [HttpPost("avatar")]
        public IActionResult UpdateAvatar(IFormFile file)
        {
            var Path = Upload.UploadFile(file);
            if (Path is null)
            {
                return BadRequest(Message.GetMessage("Ha sorgit un error al intentar pujar la imatge en el servidor."));
            }

            User user = userGetter.GetUser();
            if (user is null)
            {
                return BadRequest(Message.GetMessage("No hi ha cap usuari connectat. Connecta't per poder canviar la imatge de perfil."));
            }

            user.Avatar = Path;
            db.DbUsers.Update(user);
            db.SaveChanges();

            return new JsonResult(Message.GetMessage("S'ha actualitzat el avatar correctament."));
        }

        public void AddBillOptions(Forum forum, Wallet wallet)
        {
            List<Transaction> Transactions = db.Transactions.Where(x => x.ForumId == forum.Id).ToList();
            foreach (Transaction transaction in Transactions)
            {
                BillOption option = new BillOption
                {
                    Name = transaction.Name,
                    Value = transaction.Value,
                    Wallet = wallet
                };
                db.BillOptions.Add(option);
            }
            db.SaveChanges();
        }

        public User UpdateUser(User UserToEdit, UserViewModel model)
        {
            if (model.Description != null)
            {
                if (!(model.Description.Length > 1000))
                {
                    UserToEdit.Description = model.Description;
                }
            }
            if (model.FacebookUrl != null)
            {
                UserToEdit.FacebookUrl = model.FacebookUrl;
            }
            if (model.RepositoryUrl != null)
            {
                UserToEdit.RepositoryUrl = model.RepositoryUrl;
            }
            if (model.TwitterUrl != null)
            {
                UserToEdit.TwitterUrl = model.TwitterUrl;
            }
            if (model.WebsiteUrl != null)
            {
                UserToEdit.WebsiteUrl = model.WebsiteUrl;
            }
            return UserToEdit;
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            var userToVerify = await userManager.FindByNameAsync(userName);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            if (await userManager.CheckPasswordAsync(userToVerify, password))
            {
                return await Task.FromResult(jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id));
            }
            return await Task.FromResult<ClaimsIdentity>(null);
        }
    }
}
