using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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

        public ForumsController(DbHandler _db, IHttpContextAccessor httpContextAccessor)
        {
            db = _db;
            claimsPrincipal = httpContextAccessor.HttpContext.User;
        }

        [HttpGet("get")]
        public IEnumerable<Forum> Get()
        {
            try
            {
                return db.Forums.ToArray();
            } catch (Exception)
            {
                return null;
            }
        }

        [HttpGet("get/{userid}")]
        public IEnumerable<Forum> GetFromUser(string userid)
        {
            var Forums = db.Forums.Where(x => x.Owner.IdentityId == userid).ToList();
            return Forums;
        }

        [HttpPost("subscribe")]
        public IActionResult Subscribe(string ForumId, string ProjectId)
        {
            List<object> Errors = new List<object>();
            Project ProjectToSearch = db.Projects.Where(x => x.Id.ToString() == ProjectId).FirstOrDefault();
            if (ProjectToSearch is null)
            {
                Errors.Add(new { Message = "No existeix aquest project" });
                return BadRequest(Errors);
            }

            Forum ForumToSearch = db.Forums.Where(x => x.Id.ToString() == ForumId).FirstOrDefault();
            if (ForumToSearch is null)
            {
                Errors.Add(new { Message = "No existeix aquest forum." });
                return BadRequest(Errors);
            }

            ProjectToSearch.Forum = ForumToSearch;
            db.Update(ProjectToSearch);
            db.SaveChanges();

            var Message = new
            {
                Message = "S'ha subscrit aquest projecte al forum correctament."
            };
            return new JsonResult(Message);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]ForumViewModel model)
        {
            List<object> Errors = new List<object>();

            User user = null;
            try
            {
                var userId = claimsPrincipal.Claims.Single(c => c.Type == "id");
                user = await db.DbUsers.SingleAsync(c => c.IdentityId == userId.Value);
            } catch (Exception)
            {
                Errors.Add(new { Message = "El usuari que intenta crear el forum és incorrecte." });
                return BadRequest(Errors);
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

            var Missatge = new
            {
                Message = "El Forum s'ha afegit Correctament"
            };
            return new OkObjectResult(Missatge);
        }

        [HttpPost("update")]
        public IActionResult Update([FromBody]Forum forum)
        {
            List<object> Errors = new List<object>();

            Forum ForumToTest = db.Forums.Where(x => x.Id == forum.Id).FirstOrDefault();

            if (!ValidateUser(ForumToTest))
            {
                Errors.Add(new { Message = "El usuari que intenta editar aquest projecte és incorrecte o el forum que s'intenta editar no existeix." });
                return BadRequest(Errors);
            }

            Errors = ValidateForum(forum);
            if (Errors.Count >= 1)
            {
                return BadRequest(Errors);
            }

            Forum ForumToEdit = db.Forums.Where(x => x.Id == forum.Id).FirstOrDefault();
            if (ForumToEdit is null)
            {
                Errors.Add(new { Message = "El forum que s'intenta editar és incorrecte." });
                return BadRequest(Errors);
            }

            ForumToEdit.Name = forum.Name;
            ForumToEdit.Title = forum.Title;
            ForumToEdit.Description = forum.Description;
            ForumToEdit.Icon = forum.Icon;
            ForumToEdit.Background = forum.Background;

            db.Forums.Update(ForumToEdit);
            db.SaveChanges();


            var Missatge = new { Missatge = "El forum s'ha editat correctament." };
            return new JsonResult(Missatge);
        }

        [HttpPost("delete")]
        public IActionResult Delete([FromBody]Forum forum)
        {
            List<object> Errors = new List<object>();

            if (!ValidateUser(forum))
            {
                Errors.Add(new { Message = "El usuari que intenta esborrar aquest forum és incorrecte" });
                return BadRequest(Errors);
            }

            var ForumToDelete = db.Forums.Where(x => x.Id == forum.Id).FirstOrDefault();
            if (ForumToDelete is null)
            {
                Errors.Add(new { Message = "El forum que s'intenta eliminar no existeix." });
                return BadRequest(Errors);
            }

            db.Forums.Remove(ForumToDelete);
            db.SaveChanges();

            var Message = new
            {
                Message = "S'ha eliminat el forum correctament."
            };

            return new JsonResult(Message);
        }

        [HttpPost("select")]
        public IActionResult Select([FromBody]Forum forum)
        {
            List<object> Errors = new List<object>();

            var ForumToSelect = db.Forums.Where(x => x.Id == forum.Id).FirstOrDefault();
            if (ForumToSelect is null)
            {
                Errors.Add(new { Message = "El forum que s'intenta seleccionar no existeix." });
            }

            return new JsonResult(ForumToSelect);
        }

        public List<object> ValidateForum(Forum forum)
        {
            List<object> Errors = new List<object>();
            if (forum.Title is null)
            {
                Errors.Add(new { Message = "No s'ha introduit cap títol al forum, Introdueix un títol." });
            }
            if (forum.Name is null)
            {
                Errors.Add(new { Message = "No s'ha introduit cap nom al forum, Introdueix un nom." });
            }
            if (forum.Description is null)
            {
                Errors.Add(new { Message = "No s'ha introduit cap descripció, introdueix una breu descripció sobre el forum." });
            }
            return Errors;
        }

        public bool ValidateUser(Forum forum)
        {
            User user = null;
            try
            {
                var userId = claimsPrincipal.Claims.Single(c => c.Type == "id");
                user = db.DbUsers.SingleAsync(c => c.IdentityId == userId.Value).GetAwaiter().GetResult();
                return (forum.OwnerId == user.Id) ? true : false;
            } catch (Exception)
            {
                return false;
            }
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