using System;
using System.Diagnostics;
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
    [Route("api/projects")]
    public class ProjectsController : Controller
    {
        private readonly DbHandler db;
        private readonly ClaimsPrincipal claimsPrincipal;

        public ProjectsController(DbHandler _db, ClaimsPrincipal _claimsPrincipal)
        {
            db = _db;
            claimsPrincipal = _claimsPrincipal;
        }

        [HttpGet("get")]
        public IEnumerable<Project> Get()
        {
            try {
                return db.Projects.ToArray();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            return null;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]ProjectViewModel model)
        {
            List<object> Errors = new List<object>();

            var userId = claimsPrincipal.Claims.Single(c => c.Type == "id");
            var user = await db.DbUsers.Include(c => c.Identity).SingleAsync(c => c.Identity.Id == userId.Value);

            if (user is null)
            {
                Errors.Add(new { Message = "El usuari que intenta crear el projecte és incorrecte." });
                return BadRequest(Errors);
            }

            Project project = new Project
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description,
                Title = model.Title,
                Owner = user
            };

            Errors = ValidateProject(project);
            if (Errors.Count >= 1)
            {
                return BadRequest(Errors);
            }

            db.Projects.Add(project);
            db.SaveChanges();

            var Missatge = new
            {
                Message = "S'ha creat el projecte correctament"
            };
            return new JsonResult(Missatge);
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody]Project project)
        {
            List<object> Errors = ValidateProject(project);

            if (Errors.Count >= 1)
            {
                return BadRequest(Errors);
            }

            Project ProjectToEdit = db.Projects.Where(x => x.Id == project.Id).FirstOrDefault();
            if (ProjectToEdit is null)
            {
                Errors.Add(new { Message = "El projecte que s'intenta editar és incorrecte." });
                return BadRequest(Errors);
            }

            ProjectToEdit.Name = project.Name;
            ProjectToEdit.Title = project.Title;
            ProjectToEdit.Description = project.Description;
            ProjectToEdit.Icon = project.Icon;
            ProjectToEdit.Background = project.Background;

            var Missatge = new { Missatge = "El projecte s'ha editat correctament." };
            return new JsonResult(Missatge);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody]Forum project)
        {
            List<object> Errors = new List<object>();

            Project ProjectToDelete = db.Projects.Where(x => x.Id == project.Id).FirstOrDefault();
            if (ProjectToDelete is null)
            {
                Errors.Add(new { Message = "El forum que s'intenta eliminar no existeix." });
                return BadRequest(Errors);
            }

            db.Projects.Remove(ProjectToDelete);
            db.SaveChanges();

            var Message = new
            {
                Message = "S'ha eliminat el projecte correctament."
            };

            return new JsonResult(Message);
        }

        [HttpPost("select")]
        public async Task<IActionResult> Select([FromBody]Project project)
        {
            List<object> Errors = new List<object>();

            var ProjectToSelect = db.Projects.Where(x => x.Id == project.Id).FirstOrDefault();
            if (ProjectToSelect is null)
            {
                Errors.Add(new { Message = "El projecte que s'intenta seleccionar no existeix." });
            }

            return new JsonResult(ProjectToSelect);
        }

        private List<object> ValidateProject(Project project)
        {
            List<object> Errors = new List<object>();
            if (project.Name is null)
            {
                Errors.Add(new { Message = "S'ha deixat el camp de nom buit." });
            }
            if (project.Description is null)
            {
                Errors.Add(new { Message = "S'ha deixat el camp de descripció buit." });
            }
            if (project.Title is null)
            {
                Errors.Add(new { Message = "S'ha deixat el camp de títol buit." });
            }
            return Errors;
        }

    }
}