using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/key")]
    public class ProjectController : Controller
    {
        private Context _context;

        public ProjectController(Context context)
        {
            _context = context;
        }

        [HttpGet, Authorize]
        public IEnumerable<Project> Get()
        {
            return _context.Projects.ToList();
        }
    } 
}