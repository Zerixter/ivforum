using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IVForum.API.Data;
using IVForum.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IVForum.API.Controllers
{
    [Authorize(Policy = "ApiUser")]
    [Route("api/values")]
    [Produces("application/json")]
    public class ValuesController : Controller
    {
        private readonly ClaimsPrincipal _caller;
        private readonly DbHandler db;

        public ValuesController(UserManager<UserModel> userManager, DbHandler _db, IHttpContextAccessor httpContextAccessor)
        {
            _caller = httpContextAccessor.HttpContext.User;
            db = _db;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = _caller.Claims.Single(c => c.Type == "id");
            var customer = await db.DbUsers.Include(c => c.Identity).SingleAsync(c => c.Identity.Id == userId.Value);

            var json_obj = new
            {
                Message = "Logeado",
                customer.Identity.Name,
                customer.Identity.Surname,
                customer.Identity.Id
            };

            return new OkObjectResult(json_obj);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}