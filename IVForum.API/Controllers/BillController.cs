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
    [Route("api/bill")]
    public class BillController : Controller
    {
        private readonly DbHandler db;
        private readonly ClaimsPrincipal claimsPrincipal;

        public BillController(DbHandler _db, IHttpContextAccessor httpContextAccessor)
        {
            db = _db;
            claimsPrincipal = httpContextAccessor.HttpContext.User;
        }

        [HttpPost("vote")]
        public IActionResult Vote(Project project, Bill bill)
        {
            List<object> Errors = new List<object>();

            User user = null;
            try
            {
                var userId = claimsPrincipal.Claims.Single(c => c.Type == "id");
                user = db.DbUsers.SingleAsync(c => c.IdentityId == userId.Value).GetAwaiter().GetResult();
            } catch (Exception)
            {
                Errors.Add(new { Message = "No hi ha cap usuari loguejat." });
                return BadRequest(Errors);
            }

            Project ProjectToValidate = db.Projects.Where(x => x.Id == project.Id).FirstOrDefault();
            if (ProjectToValidate is null)
            {
                Errors.Add(new { Message = "Aquest projecte no existeix." });
                return BadRequest(Errors);
            }

            Bill BillToValidate = db.Bills.Where(x => x.Id == bill.Id).FirstOrDefault();
            if (BillToValidate is null)
            {
                Errors.Add(new { Message = "Aquest bitllet no existeix" });
                return BadRequest(Errors);
            }

            Wallet WalletToValidate = db.Wallets.Where(x => x.ForumId == ProjectToValidate.ForumId && x.UserId == user.Id).FirstOrDefault();
            if (WalletToValidate is null)
            {
                Errors.Add(new { Message = "Aquest wallet no existeix." });
                return BadRequest(Errors);
            }

            BillOption billOption = db.BillOptions.Where(x => x.WalletId == WalletToValidate.Id && x.Value == bill.Value).FirstOrDefault();
            if (billOption is null)
            {
                Errors.Add(new { Message = "Aquest bill option no existeix" });
                return BadRequest(Errors);
            }

            Vote vote = new Vote
            {
                Name = bill.Name,
                Value = bill.Value,
                Project = ProjectToValidate
            };

            db.Votes.Add(vote);
            db.BillOptions.Remove(billOption);
            db.SaveChanges();

            var Message = new
            {
                Message = "S'ha realitzat el vot correctament"
            };
            return new JsonResult(Message);
        }
    }
}