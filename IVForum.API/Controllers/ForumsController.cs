using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IVForum.API.Classes;
using IVForum.API.Data;
using IVForum.API.Models;
using IVForum.API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IVForum.API.Controllers
{
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

        [HttpGet("get")]
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

        [HttpGet("get/{userid}")]
        public IEnumerable<Forum> GetFromUser(string userid)
        {
            return db.Forums.Where(x => x.Owner.IdentityId == userid).ToArray();
        }

        [HttpGet("get/subscribed/{id_user}")]
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
                    Forum forum = db.Forums.Where(x => x.Id == wallet.ForumId).FirstOrDefault();
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

        [HttpGet("get/{id_forum}/projects")]
        public IEnumerable<Project> GetProjects(string id_forum)
        {
            try
            {
                Forum forum = db.Forums.FirstOrDefault(x => x.Id.ToString() == id_forum);
                return db.Projects.Where(x => x.Forum == forum).ToArray();
            } catch (Exception)
            {
                return null;
            }
        }

        [HttpPost("subscribe")]
        public IActionResult Subscribe([FromBody]SubscriptionViewModel model)
        {
            List<object> Errors = new List<object>();

            if (model.ForumId is null)
            {
                Errors.Add(Message.GetMessage("S'ha de introduir la id del Forum."));
            }
            if (model.ProjectId is null)
            {
                Errors.Add(Message.GetMessage("S'ha de introduir la id del Projecet"));
            }

            if (Errors.Count > 0)
            {
                return BadRequest(Errors);
            }

            Project ProjectToSearch = db.Projects.Where(x => x.Id.ToString() == model.ProjectId).FirstOrDefault();
            if (ProjectToSearch is null)
            {
                Errors.Add(Message.GetMessage("No existeix aquest project"));
                return BadRequest(Errors);
            }

            Forum ForumToSearch = db.Forums.Where(x => x.Id.ToString() == model.ForumId).FirstOrDefault();
            if (ForumToSearch is null)
            {
                Errors.Add(Message.GetMessage("No existeix aquest forum."));
                return BadRequest(Errors);
            }

            ProjectToSearch.Forum = ForumToSearch;
            db.Update(ProjectToSearch);
            db.SaveChanges();

            return new JsonResult(Message.GetMessage("S'ha subscrit aquest projecte al forum correctament."));
        }

        [HttpPost("create")]
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

            Errors = ValidateForum(forum);
            if (Errors.Count >= 1)
            {
                BadRequest(Errors);
            }

            db.Forums.Add(forum);
            db.SaveChanges();

            GetTransactions(forum);
            return new OkObjectResult(Message.GetMessage("El Forum s'ha afegit Correctament"));
        }

        [HttpPost("view")]
        public IActionResult View([FromBody]ViewsViewModel model)
        {
            Forum forum = db.Forums.FirstOrDefault(x => x.Id.ToString() == model.ForumId);
            if (forum is null)
            {
                return BadRequest(Message.GetMessage("El forum no existeix"));
            }

            forum.Views++;
            db.Forums.Update(forum);
            db.SaveChanges();
            
            return new OkObjectResult(null);
        }

        [HttpPost("update")]
        public IActionResult Update([FromBody]Forum forum)
        {
            List<object> Errors = new List<object>();
            Forum ForumToTest = db.Forums.Where(x => x.Id == forum.Id).FirstOrDefault();

            if (!ValidateUser(ForumToTest))
            {
                Message.GetMessage("El usuari que intenta editar aquest projecte és incorrecte o el forum que s'intenta editar no existeix.");
            }

            Errors = ValidateForum(forum);
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

        [HttpPost("delete")]
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

        [HttpPost("select")]
        public IActionResult Select([FromBody]Forum forum)
        {
            List<object> Errors = new List<object>();

            var ForumToSelect = db.Forums.Where(x => x.Id == forum.Id).Include(x => x.Owner).FirstOrDefault();
            if (ForumToSelect is null)
            {
                Errors.Add(Message.GetMessage("El forum que s'intenta seleccionar no existeix."));
            }

            return new JsonResult(ForumToSelect);
        }

        public List<object> ValidateForum(Forum forum)
        {
            List<object> Errors = new List<object>();
            if (forum.Title is null)
            {
                Errors.Add(Message.GetMessage("No s'ha introduit cap títol al forum, Introdueix un títol."));
            }
            if (forum.Name is null)
            {
                Errors.Add(Message.GetMessage("No s'ha introduit cap nom al forum, Introdueix un nom."));
            }
            if (forum.Description is null)
            {
                Errors.Add(Message.GetMessage("No s'ha introduit cap descripció, introdueix una breu descripció sobre el forum."));
            }
            if (forum.Description.Length > 1000)
            {
                Errors.Add(Message.GetMessage("La llargada de la descripció no pot ser més llarga de 1000 carácters."));
            }
            return Errors;
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