using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVForum.API.Classes;
using IVForum.API.Data;
using IVForum.API.Models;
using IVForum.API.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IVForum.API.Controllers
{
    [Route("api/transaction")]
    public class TransactionController : Controller
    {
        private readonly DbHandler db;
        private UserGetter userGetter;

        public TransactionController(DbHandler _db, IHttpContextAccessor httpContextAccessor)
        {
            db = _db;
            userGetter = new UserGetter(_db, httpContextAccessor);
        }

        [HttpPost("vote")]
        public IActionResult Vote([FromBody]VoteViewModel model)
        {
            
            Project ProjectToSearch = db.Projects.Where(x => x.Id.ToString() == model.ProjectId).Include(x => x.Forum).FirstOrDefault();
            if (ProjectToSearch is null)
            {
                return BadRequest(Message.GetMessage("Aquest projecte no existeix."));
            }

            Forum ForumToSearch = ProjectToSearch.Forum;
            if (ForumToSearch is null)
            {
                return BadRequest(Message.GetMessage("Aquest projecte no esta inscrit a cap forum per poder votar-lo."));
            }

            User user = userGetter.GetUser();
            if (user is null)
            {
                return BadRequest(Message.GetMessage("T'has de loguejar per poder votar."));
            }

            Wallet WalletToSearch = db.Wallets.Where(x => x.ForumId == ForumToSearch.Id && x.UserId == user.Id).FirstOrDefault();
            if (WalletToSearch is null)
            {
                return BadRequest(Message.GetMessage("El usuari no participa en el forum per podar votar en el projecte."));
            }

            BillOption BillToSearch = db.BillOptions.Where(x => x.Value == int.Parse(model.Value) && x.WalletId == WalletToSearch.Id).FirstOrDefault();
            if (BillToSearch is null)
            {
                return BadRequest(Message.GetMessage("El usuari no te aquesta opció de vot."));
            }

            /*if (ForumToSearch.DateBeginsVote.Date >= DateTime.Now.Date && DateTime.Now.Date <= ForumToSearch.DateEndsVote.Date)
            {*/
                Vote vote = new Vote
                {
                    ImgUri = BillToSearch.ImgUri,
                    Name = BillToSearch.Name,
                    Value = BillToSearch.Value,
                    Project = ProjectToSearch
                };

                ProjectToSearch.TotalMoney += vote.Value;

                db.Remove(BillToSearch);
                db.Votes.Add(vote);
                db.Projects.Update(ProjectToSearch);
                db.SaveChanges();

                return new JsonResult(Message.GetMessage("El vot s'ha realitzat correctament."));
            //}
            return BadRequest(Message.GetMessage("No s'ha pogut realitzar el vot perquè la data actual no està dintre del termini de vot del Forum."));
        }
    }
}
