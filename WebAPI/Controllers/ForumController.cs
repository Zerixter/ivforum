using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WebAPI.Models;
using WebAPI.Models.ViewModels;

namespace WebAPI.Controllers
{
	[Authorize]
	[Route("api/forums")]
	[Produces("application/json")]
	public class ForumController : Controller
    {
		[HttpPost]
		public void AddForum(AddForumViewModel forum)
		{
			//...
		}

		[HttpPost]
		public void RemoveForum(Forum forum)
		{
			//...
		}

		[HttpPut]
		public void EditForum(Forum forum)
		{
			//...
		}

		[HttpGet]
		public void ShowForumInfo(Forum forum)
		{
			//...
		}

		[HttpPost]
		public void AddProjectToForum(Forum forum, Project project)
		{
			//...
		}
    }
}