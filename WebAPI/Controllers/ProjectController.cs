using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WebAPI.Models;
using WebAPI.Models.ViewModels;

namespace WebAPI.Controllers
{
	[Authorize]
	[Route("api/projects")]
	[Produces("application/json")]
    public class ProjectController : Controller
    {
        private ApplicationDbContext db;
		public ProjectController(ApplicationDbContext context) => db = context;

		[HttpPost]
		public void AddProject(AddProjectViewModel project)
		{
			//...
		}

		[HttpPost]
		public void RemoveProject(Project project)
		{
			//..
		}

		[HttpPut]
		public void UpdateProject(Project project)
		{
			//...
		}

		[HttpGet]
		public void ShowProjectDetails(Project project)
		{
			//...
		}
    } 
}