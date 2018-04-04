using IVForum.API.Auth;
using IVForum.API.Classes;
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
using System.Linq;
using System.Security.Claims;
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
        private readonly UserGetter userGetter;

        public AccountController(UserManager<UserModel> _userManager, IJwtFactory _jwtFactory, IOptions<JwtIssuerOptions> _jwtOptions, DbHandler _db, IHttpContextAccessor httpContextAccessor)
        {
            db = _db;
            userGetter = new UserGetter(db, httpContextAccessor);
            userManager = _userManager;
            jwtFactory = _jwtFactory;
            jwtOptions = _jwtOptions.Value;
        }

        [HttpGet]
        public IActionResult Get()
        {
            User user = userGetter.GetUser();
            if (user is null)
            {
                return BadRequest(Message.GetMessage("No hi ha cap usuari connectat per poder visualtizar les dades."));
            }

            UserViewModel model = new UserViewModel
            {
                Id = user.Id,
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
            return new JsonResult(model);
        }

        [HttpGet("{id_user}")]
        public IActionResult Get(string id_user)
        {
            User user = userGetter.GetUser(id_user);
            if (user is null)
            {
                return BadRequest(Message.GetMessage("No existeix cap usuari amb aquesta id en la base de dades."));
            }

            UserViewModel model = new UserViewModel
            {
                Id = user.Id,
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
            return new JsonResult(model);
        }

        [HttpGet("subscribed/{id_forum}")]
        public IActionResult GetSubscribed(string id_forum)
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
            return new OkResult();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            List<object> Errors = RegisterViewModel.ValidateRegister(db, model);            

            if (Errors.Count >= 1)
            {
                return BadRequest(Errors);
            }
            try
            {
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

                var jwt = await Tokens.GenerateJwt(user.Id.ToString(), identity, jwtFactory, model.Email, jwtOptions, jsonSerializerSettings);
                return new OkObjectResult(jwt);
            } catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]CredentialsViewModel credentials)
        {
            List<object> Errors = CredentialsViewModel.ValidateCredentials(credentials);

            if (Errors.Count > 0)
            {
                return BadRequest(Errors);
            }

            UserModel userModel = null;
            User user = null;
            var identity = await GetClaimsIdentity(credentials.Email, credentials.Password);
            if (identity is null)
            {
                userModel = db.Users.Where(x => x.UserName == credentials.Email).FirstOrDefault();
                if (userModel is null)
                {
                    Errors.Add(Message.GetMessage("El compte d'usuari introduit és incorrecte"));
                }
                else
                {
                    Errors.Add(Message.GetMessage("La contrasenya introduida no és correcte"));
                }
                return BadRequest(Errors);
            }
            userModel = db.Users.Where(x => x.UserName == credentials.Email).FirstOrDefault();
            user = db.DbUsers.Where(x => x.IdentityId == userModel.Id).FirstOrDefault();
            string user_id = "";
            if (user != null)
            {
                user_id = user.Id.ToString();
            }

            var jwt = await Tokens.GenerateJwt(user_id, identity, jwtFactory, credentials.Email, jwtOptions, jsonSerializerSettings);
            return new OkObjectResult(jwt);
        }

        [HttpPut]
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

        [HttpDelete]
        public IActionResult Delete()
        {
            User user = null;
            try
            {
                user = userGetter.GetUser();
                var aspUser = db.Users.FirstOrDefault(x => x.Id == user.IdentityId);
                
                if (aspUser != null && user != null)
                {
                    db.Remove(user);
                    db.Remove(aspUser);
                    db.SaveChanges();
                    return new JsonResult(Message.GetMessage("S'ha esborrat el usuari correctament."));
                }
                return BadRequest(Message.GetMessage("S'ha produit un error al intentar esborrar el usuari."));
            }
            catch (Exception)
            {
                return BadRequest(Message.GetMessage("S'ha produit un error al intentar esborrar el usuari (No hi ha cap usuari loguejat)."));
            }
        }

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

        // Mètodes

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

        private void AddBillOptions(Forum forum, Wallet wallet)
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

        private User UpdateUser(User UserToEdit, UserViewModel model)
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
    }
}
