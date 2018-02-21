using System;
using System.Collections.Generic;
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
    [Produces("application/json")]
    [Route("api/Forums")]
    public class ForumsController : Controller
    {
        private readonly DbHandler db;
        private readonly ClaimsPrincipal claimsPrincipal;

        public ForumsController(DbHandler _db, IHttpContextAccessor httpContextAccessor)
        {
            db = _db;
            claimsPrincipal = httpContextAccessor.HttpContext.User;
        }

        [HttpGet]
        public IEnumerable<Forum> Get()
        {
            return db.Forums.ToArray();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Post([FromBody]ForumViewModel model)
        {
            if (model.Name is null || model.Title is null || model.Description is null)
            {
                List<string> error_message = new List<string>();
                var message = "";
                if (model.Title is null)
                {
                    error_message.Add("No s'ha introduit cap títol al forum, Introdueix un títol.");
                }
                if (model.Name is null)
                {
                    error_message.Add("No s'ha introduit cap nom al forum, Introdueix un nom.");
                }
                if (model.Description is null)
                {
                    error_message.Add("\nNo s'ha introduit cap descripció, introdueix una breu descripció sobre el forum.");
                }
                return BadRequest(error_message.ToArray());
            }

            var userId = claimsPrincipal.Claims.Single(c => c.Type == "id");
            var user = await db.DbUsers.Include(c => c.Identity).SingleAsync(c => c.Identity.Id == userId.Value);

            Forum forum = new Forum
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Title = model.Title,
                Description = model.Description,
                Owner = user
            };

            db.Forums.Add(forum);
            db.SaveChanges();

            var json_object = new
            {
                Message = "El Forum s'ha afegit Correctament"
            };

            return new OkObjectResult(json_object);
        }
    }
}