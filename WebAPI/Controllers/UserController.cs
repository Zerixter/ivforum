using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WebAPI.Models;
using WebAPI.Models.ViewModels;

namespace WebAPI.Controllers
{
	[Authorize]
	[Route("api/users")]
	[Produces("application/json")]
    public class UserController : Controller
    {
		private ApplicationDbContext db;
		public UserController(ApplicationDbContext applicationDbContext) => db = applicationDbContext;

        [HttpPost]
		public void RegisterUser(AddUserViewModel user)
		{
			//...
		}

		[HttpPost]
		public void DeleteUserAccount(User user)
		{
			//...
		}

		[HttpPut]
		public void UpdateUser(User user)
		{
			//...
		}

		[HttpGet]
		public void ShowUserInfo(User user)
		{
			//...
		}

		[HttpPut]
		public void UpdateSocialInfo(SocialViewModel social)
		{
			//...
		}
    }
}