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
        public IEnumerable<ProjectListViewModel> Get()
        {
            try {
                return db.Projects.Join(db.Users, x => x.Owner.IdentityId, us => us.Id, (x, us) => new ProjectListViewModel
                {
                    Id = x.Id.ToString(),
                    Title = x.Title,
                    Description = x.Description,
                    Background = x.Background,
                    CreationDate = x.CreationDate,
                    TotalMoney = x.TotalMoney,
                    RepositoryUrl = x.RepositoryUrl,
                    WebsiteUrl = x.WebsiteUrl,
                    Forum = x.Forum,
                    Views = x.Views,
                    Owner = new UserViewModel
                    {
                        Id = x.Owner.Id,
                        Avatar = x.Owner.Avatar,
                        Description = x.Owner.Description,
                        WebsiteUrl = x.Owner.WebsiteUrl,
                        RepositoryUrl = x.Owner.RepositoryUrl,
                        FacebookUrl = x.Owner.FacebookUrl,
                        TwitterUrl = x.Owner.TwitterUrl,
                        Name = us.Name,
                        Surname = us.Surname,
                        Email = us.Email
                    }
                }).ToArray();
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        [HttpGet("{userid}")]
        public IEnumerable<ProjectListViewModel> GetFromUser([FromRoute]string userid)
        {
            return db.Projects.Join(db.Users, x => x.Owner.IdentityId, us => us.Id, (x, us) => new ProjectListViewModel
            {
                Id = x.Id.ToString(),
                Title = x.Title,
                Description = x.Description,
                Background = x.Background,
                CreationDate = x.CreationDate,
                TotalMoney = x.TotalMoney,
                RepositoryUrl = x.RepositoryUrl,
                WebsiteUrl = x.WebsiteUrl,
                Forum = x.Forum,
                Views = x.Views,
                Owner = new UserViewModel
                {
                    Id = x.Owner.Id,
                    Avatar = x.Owner.Avatar,
                    Description = x.Owner.Description,
                    WebsiteUrl = x.Owner.WebsiteUrl,
                    RepositoryUrl = x.Owner.RepositoryUrl,
                    FacebookUrl = x.Owner.FacebookUrl,
                    TwitterUrl = x.Owner.TwitterUrl,
                    Name = us.Name,
                    Surname = us.Surname,
                    Email = us.Email
                }
            }).Where(x => x.Owner.Id.ToString() == userid).ToArray();
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
                Description = model.Description,
                Title = model.Title,
                Background = model.Background,
                Icon = model.Icon,
                RepositoryUrl = model.RepositoryUrl,
                WebsiteUrl = model.WebsiteUrl,
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

        [HttpPut("view/{id_project}")]
        public IActionResult ViewProject([FromRoute]string id_project)
        {
            Project project = db.Projects.FirstOrDefault(x => x.Id.ToString() == id_project);
            if (project is null)
            {
                return BadRequest(Message.GetMessage("El projecte no existeix"));
            }

            project.Views++;
            db.Projects.Update(project);
            db.SaveChanges();

            return new JsonResult(null);
        }

        [HttpDelete("{id_project}")]
        public IActionResult Delete([FromRoute]string id_project)
        {
            Project ProjectToDelete = db.Projects.Where(x => x.Id.ToString() == id_project).Include(x => x.Owner).FirstOrDefault();
            if (ProjectToDelete is null)
            {
                return BadRequest(Message.GetMessage("El forum que s'intenta eliminar no existeix."));
            }

            if (!ValidateUser(ProjectToDelete))
            {
                return BadRequest(Message.GetMessage("El usuari que intenta esborrar aquest projecte és incorrecte"));
            }

            db.Projects.Remove(ProjectToDelete);
            db.SaveChanges();

            return new JsonResult(Message.GetMessage("S'ha eliminat el projecte correctament."));
        }

        private Project UpdateProject(Project ProjectToEdit, Project project)
        {
            if (project.Title != null)
            {
                if (!(project.Title.Length > 100))
                {
                    ProjectToEdit.Title = project.Title;
                }
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