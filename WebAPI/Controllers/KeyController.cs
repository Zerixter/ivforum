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
    public class KeyController : Controller
    {
        private Context _context;

        public KeyController(Context context)
        {
            _context = context;
        }

        [HttpGet, Authorize]
        public string Get()
        {
            var pipo = new
            {
                hamza = "hola",
                lolo = "asda"
            };
            var currentUser = HttpContext.User;
            return pipo.ToString();
        }
    } 
}