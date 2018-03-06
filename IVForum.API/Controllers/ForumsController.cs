using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IVForum.API.Classes;
using IVForum.API.Data;
using IVForum.API.Models;
using IVForum.API.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IVForum.API.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [Route("api/forum")]
    public class ForumsController : Controller
    {
        private readonly DbHandler db;
        private readonly ClaimsPrincipal claimsPrincipal;
        private readonly UserGetter userGetter;

        public ForumsController(DbHandler _db, IHttpContextAccessor httpContextAccessor)
        {
            db = _db;
            claimsPrincipal = httpContextAccessor.HttpContext.User;
            userGetter = new UserGetter(db, httpContextAccessor);
        }

        [HttpGet]
        public IEnumerable<Forum> Get()
        {
            try
            {
                return db.Forums.Include(x => x.Owner).ToArray();
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet("{id_user}")]
        public IEnumerable<Forum> GetFromUser(string id_user)
        {
            return db.Forums.Where(x => x.Owner.IdentityId == id_user).Include(x => x.Owner).ToArray();
        }

        [HttpGet("user/{id_user}")]
        public IEnumerable<Forum> GetPersonal(string id_user)
        {
            return db.Forums.Where(x => x.Owner.Id.ToString() == id_user).Include(x => x.Owner).ToArray();
        }

        [HttpGet("subscribed/{id_user}")]
        public IEnumerable<Forum> GetForumsSubscribedUser(string id_user)
        {
            User user = null;
            try
            {
                user = userGetter.GetUser(id_user);
                List<Wallet> Wallets = db.Wallets.Where(x => x.User.Id == user.Id).Include(x => x.User).ToList();
                List<Forum> Forums = new List<Forum>();
                foreach (var wallet in Wallets)
                {
                    Forum forum = db.Forums.Where(x => x.Id == wallet.ForumId).Include(x => x.Owner).FirstOrDefault();
                    if (forum != null)
                    {
                        Forums.Add(forum);
                    }
                }
                return Forums;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet("select/{id_forum}")]
        public IActionResult Select(string id_forum)
        {
            List<object> Errors = new List<object>();

            var ForumToSelect = db.Forums.Where(x => x.Id.ToString() == id_forum).Include(x => x.Owner).FirstOrDefault();
            if (ForumToSelect is null)
            {
                Errors.Add(Message.GetMessage("El forum que s'intenta seleccionar no existeix."));
            }

            return new JsonResult(ForumToSelect);
        }

        [HttpPost]
        public IActionResult Create([FromBody]ForumViewModel model)
        {
            List<object> Errors = new List<object>();

            User user = userGetter.GetUser();
            if (user is null)
            {
                return BadRequest(Message.GetMessage("El usuari que intenta crear el forum és incorrecte."));
            }

            Forum forum = new Forum
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Title = model.Title,
                Description = model.Description,
                Owner = user
            };

            Errors = Forum.ValidateForum(forum);
            if (Errors.Count >= 1)
            {
                BadRequest(Errors);
            }

            db.Forums.Add(forum);
            db.SaveChanges();

            GetTransactions(forum);
            return new OkObjectResult(Message.GetMessage("El Forum s'ha afegit Correctament"));
        }

        [HttpPut]
        public IActionResult Update([FromBody]Forum forum)
        {
            List<object> Errors = new List<object>();
            Forum ForumToTest = db.Forums.Where(x => x.Id == forum.Id).FirstOrDefault();

            if (!ValidateUser(ForumToTest))
            {
                Message.GetMessage("El usuari que intenta editar aquest projecte és incorrecte o el forum que s'intenta editar no existeix.");
            }

            Errors = Forum.ValidateForum(forum);
            if (Errors.Count >= 1)
            {
                return BadRequest(Errors);
            }

            Forum ForumToEdit = db.Forums.Where(x => x.Id == forum.Id).FirstOrDefault();
            if (ForumToEdit is null)
            {
                return BadRequest(Message.GetMessage("El forum que s'intenta editar és incorrecte."));
            }

            ForumToEdit = UpdateForum(ForumToEdit, forum);

            db.Forums.Update(ForumToEdit);
            db.SaveChanges();

            return new JsonResult(Message.GetMessage("El forum s'ha editat correctament."));
        }

        [HttpGet("view")]
        public IActionResult ViewForum([FromBody]string id_forum)
        {
            Forum forum = db.Forums.FirstOrDefault(x => x.Id.ToString() == id_forum);
            if (forum is null)
            {
                return BadRequest(Message.GetMessage("El forum no existeix"));
            }

            forum.Views++;
            db.Forums.Update(forum);
            db.SaveChanges();

            return new JsonResult(null);
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]Forum forum)
        {
            List<object> Errors = new List<object>();

            if (!ValidateUser(forum))
            {
                Errors.Add(Message.GetMessage("El usuari que intenta esborrar aquest forum és incorrecte"));
                return BadRequest(Errors);
            }

            var ForumToDelete = db.Forums.Where(x => x.Id == forum.Id).FirstOrDefault();
            if (ForumToDelete is null)
            {
                Errors.Add(Message.GetMessage("El forum que s'intenta eliminar no existeix."));
                return BadRequest(Errors);
            }

            db.Forums.Remove(ForumToDelete);
            db.SaveChanges();

            return new JsonResult(Message.GetMessage("S'ha eliminat el forum correctament."));
        }

        public bool ValidateUser(Forum forum)
        {
            User user = userGetter.GetUser();
            try
            {
                return (forum.Owner.Id == user.Id) ? true : false;
            } catch (Exception)
            {
                return false;
            }
        }

        public Forum UpdateForum(Forum ForumToEdit, Forum forum)
        {
            if (forum.Name != null)
            {
                ForumToEdit.Name = forum.Name;
            }
            if (forum.Title != null)
            {
                ForumToEdit.Title = forum.Title;
            }
            if (forum.Description != null)
            {
                if (!(forum.Description.Length > 1000))
                {
                    ForumToEdit.Description = forum.Description;
                }
            }
            if (forum.Icon != null)
            {
                ForumToEdit.Icon = forum.Icon;
            }
            if (forum.Icon != null)
            {
                ForumToEdit.Background = forum.Background;
            }
            return ForumToEdit;
        }

        public void GetTransactions(Forum forum)
        {
            db.Transactions.Add(new Transaction
            {
                Forum = forum,
                Name = "20",
                Value = 20
            });

            db.Transactions.Add(new Transaction
            {
                Forum = forum,
                Name = "50",
                Value = 50
            });

            db.Transactions.Add(new Transaction
            {
                Forum = forum,
                Name = "100",
                Value = 100
            });

            db.SaveChanges();
        }
    }
}