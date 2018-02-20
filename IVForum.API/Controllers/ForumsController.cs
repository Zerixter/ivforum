using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVForum.API.Data;
using IVForum.API.Models;
using IVForum.API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IVForum.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Forums")]
    public class ForumsController : Controller
    {
        private readonly DbHandler db;

        public ForumsController(DbHandler _db)
        {
            db = _db;
        }

        [HttpGet]
        public IEnumerable<Forum> Get()
        {
            return db.Forums.ToArray();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]ForumViewModel model)
        {


            if (!ModelState.IsValid)
            {

            }

            return new OkObjectResult(model);
        }
    }
}