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

namespace IVForum.API.Controllers
{
    [Route("api/subscription")]
    public class SubscriptionController : Controller
    {
        private readonly DbHandler db;
        private readonly UserGetter userGetter;

        public SubscriptionController(DbHandler _db, IHttpContextAccessor httpContextAccessor)
        {
            db = _db;
            userGetter = new UserGetter(db, httpContextAccessor);
        }

        [HttpPost("subcribe/forum")]
        public IActionResult Subscribe([FromBody]ForumViewModel model)
        {
            Forum ForumToSearch = db.Forums.FirstOrDefault(x => x.Id.ToString() == model.Id);
            if (ForumToSearch is null)
            {
                return BadRequest(Message.GetMessage("No existeix cap forum amb aquesta id en la base de dades."));
            }

            User user = userGetter.GetUser();
            if (user is null)
            {
                return BadRequest(Message.GetMessage("No hi ha cap usuari connectat ara mateix. Connecta't per poder subscriure a un forum"));
            }

            Wallet wallet = new Wallet
            {
                Id = Guid.NewGuid(),
                Forum = ForumToSearch,
                User = user
            };

            db.Wallets.Add(wallet);
            db.SaveChanges();

            AddBillOptions(ForumToSearch, wallet);
            return new JsonResult(Message.GetMessage("El usuari s'ha subscrit exitosament a aquest forum."));
        }

        [HttpPost("subscribe/project")]
        public IActionResult Subscribe([FromBody]SubscriptionViewModel model)
        {
            List<object> Errors = new List<object>();

            if (model.ForumId is null)
            {
                Errors.Add(Message.GetMessage("S'ha de introduir la id del Forum."));
            }
            if (model.ProjectId is null)
            {
                Errors.Add(Message.GetMessage("S'ha de introduir la id del Projecet"));
            }

            if (Errors.Count > 0)
            {
                return BadRequest(Errors);
            }

            Project ProjectToSearch = db.Projects.Where(x => x.Id.ToString() == model.ProjectId).FirstOrDefault();
            if (ProjectToSearch is null)
            {
                Errors.Add(Message.GetMessage("No existeix aquest project"));
                return BadRequest(Errors);
            }

            Forum ForumToSearch = db.Forums.Where(x => x.Id.ToString() == model.ForumId).FirstOrDefault();
            if (ForumToSearch is null)
            {
                Errors.Add(Message.GetMessage("No existeix aquest forum."));
                return BadRequest(Errors);
            }

            ProjectToSearch.Forum = ForumToSearch;
            db.Update(ProjectToSearch);
            db.SaveChanges();

            return new JsonResult(Message.GetMessage("S'ha subscrit aquest projecte al forum correctament."));
        }

        [HttpPost("unsubscribe/forum")]
        public IActionResult UnsubscribeForum([FromBody]ForumViewModel model)
        {
            Forum ForumToSearch = db.Forums.FirstOrDefault(x => x.Id.ToString() == model.Id);
            if (ForumToSearch is null)
            {
                return BadRequest(Message.GetMessage("No existeix cap forum amb aquesta id en la base de dades."));
            }

            User user = userGetter.GetUser();
            if (user is null)
            {
                return BadRequest(Message.GetMessage("No hi ha cap usuari connectat ara mateix. Connecta't per poder subscriure a un forum"));
            }

            Wallet wallet = db.Wallets.Where(x => x.User.Id == user.Id && x.Forum.Id == ForumToSearch.Id).FirstOrDefault();
            if (wallet is null)
            {
                return BadRequest(Message.GetMessage("Aquest usuari no es pot desubscriure perquè no esta subscrit al forum."));
            }

            db.Wallets.Remove(wallet);
            db.SaveChanges();

            return new JsonResult(Message.GetMessage("El usuari s'ha desubscrit exitosament a aquest forum."));
        }

        public void AddBillOptions(Forum forum, Wallet wallet)
        {
            List<Transaction> Transactions = db.Transactions.Where(x => x.ForumId == forum.Id).ToList();
            foreach (Transaction transaction in Transactions)
            {
                BillOption option = new BillOption
                {
                    Name = transaction.Name,
                    Value = transaction.Value,
                    Wallet = wallet
                };
                db.BillOptions.Add(option);
            }
            db.SaveChanges();
        }
    }
}