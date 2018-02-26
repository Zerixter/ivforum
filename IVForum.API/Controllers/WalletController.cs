using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IVForum.API.Data;
using IVForum.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IVForum.API.Controllers
{
    [Route("api/wallet")]
    public class WalletController : Controller
    {
        private readonly DbHandler db;
        private readonly ClaimsPrincipal claimsPrincipal;

        public WalletController(DbHandler _db, IHttpContextAccessor httpContextAccessor)
        {
            db = _db;
            claimsPrincipal = httpContextAccessor.HttpContext.User;
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe([FromBody]Forum forum)
        {
            List<object> Errors = new List<object>();

            Forum ForumToSearch = db.Forums.Where(x => x.Id == forum.Id).SingleOrDefault();
            if (ForumToSearch is null)
            {
                Errors.Add(new { Message = "Aquest forum no existeix." });
                return BadRequest(Errors);
            }

            User user = null;
            try
            {
                var userId = claimsPrincipal.Claims.Single(c => c.Type == "id");
                user = await db.DbUsers.SingleAsync(c => c.IdentityId == userId.Value);
            }
            catch (Exception)
            {
                Errors.Add(new { Message = "No hi ha cap usuari loguejat." });
                return BadRequest(Errors);
            }

            Wallet wallet = new Wallet
            {
                Forum = ForumToSearch,
                Owner = user
            };

            db.Wallets.Add(wallet);
            db.SaveChanges();

            var Message = new
            {
                Message = "El usuari s'ha subscrit exitosament a aquest forum."
            };
            return new JsonResult(Message);
        }
    }
}