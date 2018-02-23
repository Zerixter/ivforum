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
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return null;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]ForumViewModel model)
        {
            List<object> Errors = new List<object>();
            var userId = claimsPrincipal.Claims.Single(c => c.Type == "id");
            var user = await db.DbUsers.Include(c => c.Identity).SingleAsync(c => c.Identity.Id == userId.Value);

            if (user is null)
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

            var Missatge = new
            {
                Message = "El Forum s'ha afegit Correctament"
            };
            return new OkObjectResult(Missatge);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody]Forum forum)
        {
            List<object> Errors = ValidateForum(forum);

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

            var Missatge = new { Missatge = "El forum s'ha editat correctament." };
            return new JsonResult(Missatge);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody]Forum forum)
        {
            List<object> Errors = new List<object>();

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
        public async Task<IActionResult> Select([FromBody]Forum forum)
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
    }
}