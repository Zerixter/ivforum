using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVForum.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IVForum.API.Controllers
{
    [Route("api/wallet")]
    public class WalletController : Controller
    {
        [HttpPost("subscribe")]
        public IActionResult Subscribe([FromBody]Forum forum)
        {

            return null;
        }
    }
}