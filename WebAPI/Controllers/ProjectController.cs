using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Collections.Generic;
using System.Linq;

using WebAPI.Models;

namespace WebAPI.Controllers
{
	[Produces("application/json")]
    [Route("api/key")]
    public class ProjectController : Controller
    {
        private ApplicationDbContext ctx;

        public ProjectController(ApplicationDbContext context)
        {
            ctx = context;
        }

        [HttpGet, Authorize]
        public IEnumerable<Project> Get()
        {
			return null;
        }
    } 
}