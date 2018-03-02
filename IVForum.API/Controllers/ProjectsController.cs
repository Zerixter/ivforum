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
using IVForum.API.Classes;

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

        [HttpGet("get")]
        public IEnumerable<Project> Get()
        {
            try {
                return db.Projects.Include(x => x.Owner).ToArray();
            } catch (Exception)
            {
                return null;
            }
        }
        
        [HttpGet("get/{userid}")]
        public IEnumerable<Project> GetFromUser(string userid)
        {
            return db.Projects.Where(x => x.Owner.IdentityId == userid).Include(x => x.Owner).ToList(); ;
        }

        [HttpPost("create")]
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

            Errors = ValidateProject(project);
            if (Errors.Count >= 1)
            {
                return BadRequest(Errors);
            }

            db.Projects.Add(project);
            db.SaveChanges();

            return new JsonResult(Message.GetMessage("S'ha creat el projecte correctament"));
        }

        [HttpPost("vote")]
        public IActionResult Vote([FromBody]VoteViewModel model)
        {
            Project ProjectToSearch = db.Projects.Where(x => x.Id.ToString() == model.ProjectId).Include(x => x.Forum).FirstOrDefault();
            if (ProjectToSearch is null)
            {
                return BadRequest(Message.GetMessage("Aquest projecte no existeix."));
            }

            Forum ForumToSearch = ProjectToSearch.Forum;
            if (ForumToSearch is null)
            {
                return BadRequest(Message.GetMessage("Aquest projecte no esta inscrit a cap forum per poder votar-lo."));
            }

            User user = userGetter.GetUser();
            if (user is null)
            {
                return BadRequest(Message.GetMessage("T'has de loguejar per poder votar."));
            }

            Wallet WalletToSearch = db.Wallets.Where(x => x.ForumId == ForumToSearch.Id && x.UserId == user.Id).FirstOrDefault();
            if (WalletToSearch is null)
            {
                return BadRequest(Message.GetMessage("El usuari no participa en el forum per podar votar en el projecte."));
            }

            BillOption BillToSearch = db.BillOptions.Where(x => x.Value == int.Parse(model.Value) && x.WalletId == WalletToSearch.Id).FirstOrDefault();
            if (BillToSearch is null)
            {
                return BadRequest(Message.GetMessage("El usuari no te aquesta opció de vot."));
            }

            Vote vote = new Vote
            {
                 ImgUri = BillToSearch.ImgUri,
                 Name = BillToSearch.Name,
                 Value = BillToSearch.Value,
                 Project = ProjectToSearch
            };

            ProjectToSearch.TotalMoney += vote.Value;

            db.Remove(BillToSearch);
            db.Votes.Add(vote);
            db.Projects.Update(ProjectToSearch);
            db.SaveChanges();

            return new JsonResult(Message.GetMessage("El vot s'ha realitzat correctament."));
        }

        [HttpPost("update")]
        public IActionResult Update([FromBody]Project project)
        {
            List<object> Errors = new List<object>();
            Project ProjectToTest = db.Projects.Where(x => x.Id == project.Id).FirstOrDefault();

            if (!ValidateUser(ProjectToTest))
            {
                return BadRequest(Message.GetMessage("El usuari que intenta editar aquest projecte és incorrecte"));
            }

            Errors = ValidateProject(project);
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

        [HttpPost("delete")]
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

        [HttpPost("select")]
        public IActionResult Select([FromBody]Project project)
        {
            var ProjectToSelect = db.Projects.Where(x => x.Id == project.Id).Include(x => x.Owner).FirstOrDefault();
            if (ProjectToSelect is null)
            {
                return BadRequest(Message.GetMessage("El projecte que s'intenta seleccionar no existeix."));
            }

            return new JsonResult(ProjectToSelect);
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
            if (project.Description.Length > 1000)
            {
                Errors.Add(new { Message = "La llargada de la descripció no pot ser més llarga de 1000 carácters." });
            }
            return Errors;
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