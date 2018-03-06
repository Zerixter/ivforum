using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IVForum.API.Data;
using IVForum.API.Models;
using IVForum.API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IVForum.API.Classes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace IVForum.API.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [Route("api/project")]
    public class ProjectsController : Controller
    {
        private readonly DbHandler db;
        private readonly ClaimsPrincipal claimsPrincipal;
        private readonly UserGetter userGetter;

        public ProjectsController(DbHandler _db, IHttpContextAccessor httpContextAccessor)
        {
            db = _db;
            claimsPrincipal = httpContextAccessor.HttpContext.User;
            userGetter = new UserGetter(db, httpContextAccessor);
        }

        [HttpGet]
        public IEnumerable<Project> Get()
        {
            try {
                return db.Projects.Include(x => x.Owner).ToArray();
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        [HttpGet("{userid}")]
        public IEnumerable<Project> GetFromUser(string userid)
        {
            return db.Projects.Where(x => x.Owner.IdentityId == userid).Include(x => x.Owner).ToList(); ;
        }

        [HttpGet("user/{userid}")]
        public IEnumerable<Project> GetPersonal(string userid)
        {
            return db.Projects.Where(x => x.Owner.Id.ToString() == userid).Include(x => x.Owner).ToList(); ;
        }

        [HttpGet("select/{project_id}")]
        public IActionResult Select(string project_id)
        {
            var ProjectToSelect = db.Projects.Where(x => x.Id.ToString() == project_id).Include(x => x.Owner).FirstOrDefault();
            if (ProjectToSelect is null)
            {
                return BadRequest(Message.GetMessage("El projecte que s'intenta seleccionar no existeix."));
            }

            return new JsonResult(ProjectToSelect);
        }

        [HttpPost]
        public IActionResult Create([FromBody]ProjectViewModel model)
        {
            List<object> Errors = new List<object>();

            User user = userGetter.GetUser();
            if (user is null)
            {
                return BadRequest(Message.GetMessage("El usuari que intenta crear el projecte és incorrecte."));
            }

            Project project = new Project
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description,
                Title = model.Title,
                Owner = user
            };

            Errors = Project.ValidateProject(project);
            if (Errors.Count >= 1)
            {
                return BadRequest(Errors);
            }

            db.Projects.Add(project);
            db.SaveChanges();

            return new JsonResult(Message.GetMessage("S'ha creat el projecte correctament"));
        }

        [HttpPut]
        public IActionResult Update([FromBody]Project project)
        {
            List<object> Errors = new List<object>();
            Project ProjectToTest = db.Projects.Where(x => x.Id == project.Id).FirstOrDefault();

            if (!ValidateUser(ProjectToTest))
            {
                return BadRequest(Message.GetMessage("El usuari que intenta editar aquest projecte és incorrecte"));
            }

            Errors = Project.ValidateProject(project);
            if (Errors.Count >= 1)
            {
                return BadRequest(Errors);
            }

            Project ProjectToEdit = db.Projects.Where(x => x.Id == project.Id).FirstOrDefault();
            if (ProjectToEdit is null)
            {
                return BadRequest(Message.GetMessage("El projecte que s'intenta editar és incorrecte."));
            }

            ProjectToEdit = UpdateProject(ProjectToEdit, project);

            return new JsonResult(Message.GetMessage("El projecte s'ha editat correctament."));
        }

        [HttpPut("view")]
        public IActionResult ViewProject([FromBody]string id_project)
        {
            Project project = db.Projects.FirstOrDefault(x => x.Id.ToString() == id_project);
            if (project is null)
            {
                return BadRequest(Message.GetMessage("El projecte no existeix"));
            }

            project.Views++;
            db.Projects.Update(project);
            db.SaveChanges();

            return new OkObjectResult(null);
        }

        [HttpDelete]
        public IActionResult Delete([FromBody]Project project)
        {
            if (!ValidateUser(project))
            {
                return BadRequest(Message.GetMessage("El usuari que intenta esborrar aquest projecte és incorrecte"));
            }

            Project ProjectToDelete = db.Projects.Where(x => x.Id == project.Id).Include(x => x.Owner).FirstOrDefault();
            if (ProjectToDelete is null)
            {
                return BadRequest(Message.GetMessage("El forum que s'intenta eliminar no existeix."));
            }

            db.Projects.Remove(ProjectToDelete);
            db.SaveChanges();

            return new JsonResult(Message.GetMessage("S'ha eliminat el projecte correctament."));
        }

        private Project UpdateProject(Project ProjectToEdit, Project project)
        {
            if (project.Name != null)
            {
                ProjectToEdit.Name = project.Name;
            }
            if (project.Title != null)
            {
                ProjectToEdit.Title = project.Title;
            }
            if (project.Description != null)
            {
                if (!(project.Description.Length > 1000))
                {
                    ProjectToEdit.Description = project.Description;
                }
            }
            if (project.Icon != null)
            {
                ProjectToEdit.Icon = project.Icon;
            }
            if (project.Background != null)
            {
                ProjectToEdit.Background = project.Background;
            }
            
            return ProjectToEdit;
        }
        
        private bool ValidateUser(Project project)
        {
            User user = null;
            try
            {
                user = userGetter.GetUser();
                return (project.Owner.Id == user.Id) ? true : false;
            } catch (Exception)
            {
                return false;
            }
        }
    }
}