using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IVForum.API.Classes;
using IVForum.API.Data;
using IVForum.API.Models;
using IVForum.API.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IVForum.API.Controllers
{
    [Route("api/forum")]
    public class ForumsController : Controller
    {
        private readonly DbHandler db;
        private readonly ClaimsPrincipal claimsPrincipal;
        private readonly UserGetter userGetter;

        public ForumsController(DbHandler _db, IHttpContextAccessor httpContextAccessor)
        {
            db = _db;
            claimsPrincipal = httpContextAccessor.HttpContext.User;
            userGetter = new UserGetter(db, httpContextAccessor);
        }

        [HttpGet]
        public IEnumerable<ForumListViewModel> Get()
        {
            try
            {
                return db.Forums.Join(db.Users, x => x.Owner.IdentityId, us => us.Id, (x, us) => new ForumListViewModel
                {
                    Id = x.Id.ToString(),
                    Title = x.Title,
                    Description = x.Description,
                    Background = x.Background,
                    DateBeginsVote = x.DateBeginsVote,
                    DateEndsVote = x.DateEndsVote,
                    CreationDate = x.CreationDate,
                    Icon = x.Icon,
                    Views = x.Views,
                    Owner = new UserViewModel
                    {
                        Id = x.Owner.Id,
                        Avatar = x.Owner.Avatar,
                        Description = x.Owner.Description,
                        WebsiteUrl = x.Owner.WebsiteUrl,
                        RepositoryUrl = x.Owner.RepositoryUrl,
                        FacebookUrl = x.Owner.FacebookUrl,
                        TwitterUrl = x.Owner.TwitterUrl,
                        Name = us.Name,
                        Surname = us.Surname,
                        Email = us.Email
                    }
                }).ToArray();
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet("{id_user}")]
        public IEnumerable<ForumListViewModel> GetFromUser(string id_user)
        {
            try
            {
                return db.Forums.Join(db.Users, x => x.Owner.IdentityId, us => us.Id, (x, us) => new ForumListViewModel
                {
                    Id = x.Id.ToString(),
                    Title = x.Title,
                    Description = x.Description,
                    Background = x.Background,
                    DateBeginsVote = x.DateBeginsVote,
                    DateEndsVote = x.DateEndsVote,
                    CreationDate = x.CreationDate,
                    Icon = x.Icon,
                    Views = x.Views,
                    Owner = new UserViewModel
                    {
                        Id = x.Owner.Id,
                        Avatar = x.Owner.Avatar,
                        Description = x.Owner.Description,
                        WebsiteUrl = x.Owner.WebsiteUrl,
                        RepositoryUrl = x.Owner.RepositoryUrl,
                        FacebookUrl = x.Owner.FacebookUrl,
                        TwitterUrl = x.Owner.TwitterUrl,
                        Name = us.Name,
                        Surname = us.Surname,
                        Email = us.Email
                    }
                }).Where(x => x.Owner.Id.ToString() == id_user).ToArray();
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet("subscribed/{id_user}")]
        public IEnumerable<ForumListViewModel> GetForumsSubscribedUser(string id_user)
        {
            User user = null;
            try
            {
                user = userGetter.GetUser(id_user);
                List<Wallet> Wallets = db.Wallets.Where(x => x.User.Id.ToString() == id_user).Include(x => x.Forum).ToList();
                List<ForumListViewModel> Forums = new List<ForumListViewModel>();
                foreach (var wallet in Wallets)
                {
                    ForumListViewModel forum = db.Forums.Join(db.Users, x => x.Owner.IdentityId, us => us.Id, (x, us) => new ForumListViewModel
                    {
                        Id = x.Id.ToString(),
                        Title = x.Title,
                        Description = x.Description,
                        Background = x.Background,
                        DateBeginsVote = x.DateBeginsVote,
                        DateEndsVote = x.DateEndsVote,
                        CreationDate = x.CreationDate,
                        Icon = x.Icon,
                        Views = x.Views,
                        Owner = new UserViewModel
                        {
                            Id = x.Owner.Id,
                            Avatar = x.Owner.Avatar,
                            Description = x.Owner.Description,
                            WebsiteUrl = x.Owner.WebsiteUrl,
                            RepositoryUrl = x.Owner.RepositoryUrl,
                            FacebookUrl = x.Owner.FacebookUrl,
                            TwitterUrl = x.Owner.TwitterUrl,
                            Name = us.Name,
                            Surname = us.Surname,
                            Email = us.Email
                        }
                    }).Where(x => x.Id == wallet.Forum.Id.ToString()).FirstOrDefault();
                    if (forum != null)
                    {
                        Forums.Add(forum);
                    }
                }
                return Forums;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet("select/{id_forum}")]
        public IActionResult Select(string id_forum)
        {
            List<object> Errors = new List<object>();
            var ForumToSelect = db.Forums.Join(db.Users, x => x.Owner.IdentityId, us => us.Id, (x, us) => new ForumListViewModel
                {
                    Id = x.Id.ToString(),
                    Title = x.Title,
                    Description = x.Description,
                    Background = x.Background,
                    DateBeginsVote = x.DateBeginsVote,
                    DateEndsVote = x.DateEndsVote,
                    CreationDate = x.CreationDate,
                    Icon = x.Icon,
                    Views = x.Views,
                    Owner = new UserViewModel
                    {
                        Id = x.Owner.Id,
                        Avatar = x.Owner.Avatar,
                        Description = x.Owner.Description,
                        WebsiteUrl = x.Owner.WebsiteUrl,
                        RepositoryUrl = x.Owner.RepositoryUrl,
                        FacebookUrl = x.Owner.FacebookUrl,
                        TwitterUrl = x.Owner.TwitterUrl,
                        Name = us.Name,
                        Surname = us.Surname,
                        Email = us.Email
                    }
                }).Where(x => x.Id.ToString() == id_forum).FirstOrDefault();
            if (ForumToSelect is null)
            {
                Errors.Add(Message.GetMessage("El forum que s'intenta seleccionar no existeix."));
            }

            return new JsonResult(ForumToSelect);
        }
        
        [HttpGet("projects/{id_forum}")]
        public IEnumerable<ProjectListViewModel> GetProjects(string id_forum)
        {
            try
            {
                Forum forum = db.Forums.FirstOrDefault(x => x.Id.ToString() == id_forum);
                return db.Projects.Join(db.Users, x => x.Owner.IdentityId, us => us.Id, (x, us) => new ProjectListViewModel
                {
                    Id = x.Id.ToString(),
                    Title = x.Title,
                    Description = x.Description,
                    Background = x.Background,
                    CreationDate = x.CreationDate,
                    TotalMoney = x.TotalMoney,
                    RepositoryUrl = x.RepositoryUrl,
                    WebsiteUrl = x.WebsiteUrl,
                    Forum = x.Forum,
                    Views = x.Views,
                    Owner = new UserViewModel
                    {
                        Id = x.Owner.Id,
                        Avatar = x.Owner.Avatar,
                        Description = x.Owner.Description,
                        WebsiteUrl = x.Owner.WebsiteUrl,
                        RepositoryUrl = x.Owner.RepositoryUrl,
                        FacebookUrl = x.Owner.FacebookUrl,
                        TwitterUrl = x.Owner.TwitterUrl,
                        Name = us.Name,
                        Surname = us.Surname,
                        Email = us.Email
                    }
                }).Where(x => x.Forum == forum).ToArray();
            } catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody]ForumViewModel model)
        {
            List<object> Errors = new List<object>();

            User user = userGetter.GetUser();
            if (user is null)
            {
                return BadRequest(Message.GetMessage("El usuari que intenta crear el forum és incorrecte."));
            }

            Forum forum = null;

            try
            {
                forum = new Forum
                {
                    Id = Guid.NewGuid(),
                    Title = model.Title,
                    Description = model.Description,
                    DateBeginsVote = model.DateBeginsVote.Date,
                    DateEndsVote = model.DateEndsVote.Date,
                    Owner = user
                };
            } catch (Exception)
            {
                return BadRequest(Message.GetMessage("No se ha pogut crear el forum."));
            }


            Errors = Forum.ValidateForum(forum);
            if (Errors.Count >= 1)
            {
                return BadRequest(Errors);
            }

            db.Forums.Add(forum);
            db.SaveChanges();

            GetTransactions(forum);
            return new OkObjectResult(Message.GetMessage("El Forum s'ha afegit Correctament"));
        }

        [HttpPut]
        public IActionResult Update([FromBody]Forum forum)
        {
            List<object> Errors = new List<object>();
            Forum ForumToTest = db.Forums.Where(x => x.Id == forum.Id).FirstOrDefault();

            if (!ValidateUser(ForumToTest))
            {
                Message.GetMessage("El usuari que intenta editar aquest projecte és incorrecte o el forum que s'intenta editar no existeix.");
            }

            Errors = Forum.ValidateForum(forum);
            if (Errors.Count >= 1)
            {
                return BadRequest(Errors);
            }

            Forum ForumToEdit = db.Forums.Where(x => x.Id == forum.Id).FirstOrDefault();
            if (ForumToEdit is null)
            {
                return BadRequest(Message.GetMessage("El forum que s'intenta editar és incorrecte."));
            }

            ForumToEdit = UpdateForum(ForumToEdit, forum);

            db.Forums.Update(ForumToEdit);
            db.SaveChanges();

            return new JsonResult(Message.GetMessage("El forum s'ha editat correctament."));
        }

        [HttpPut("view/{id_forum}")]
        public IActionResult ViewForum([FromRoute]string id_forum)
        {
            Forum forum = db.Forums.FirstOrDefault(x => x.Id.ToString() == id_forum);
            if (forum is null)
            {
                return BadRequest(Message.GetMessage("El forum no existeix"));
            }

            forum.Views++;
            db.Forums.Update(forum);
            db.SaveChanges();

            return new JsonResult(null);
        }

        [HttpDelete("{id_forum}")]
        public IActionResult Delete([FromRoute]string id_forum)
        {
            Forum ForumToDelete = db.Forums.Where(x => x.Id.ToString() == id_forum).Include(x => x.Owner).FirstOrDefault();
            if (ForumToDelete is null)
            {
                return BadRequest(Message.GetMessage("El forum que s'intenta eliminar no existeix."));
            }

            if (!ValidateUser(ForumToDelete))
            {
                return BadRequest(Message.GetMessage("El usuari que intenta esborrar aquest forum és incorrecte"));
            }

            List<Transaction> Transactions = db.Transactions.Where(x => x.Forum.Id.ToString() == id_forum).Include(x => x.Forum).ToList();
            foreach (Transaction transaction in Transactions)
            {
                db.Transactions.Remove(transaction);
            }

            List<Wallet> WalletsForForum = db.Wallets.Where(x => x.Forum.Id.ToString() == id_forum).Include(x => x.Forum).ToList();
            foreach (Wallet wallet in WalletsForForum)
            {
                List<BillOption> BillOptions = db.BillOptions.Where(x => x.Wallet.Id == wallet.Id).Include(x => x.Wallet).ToList();
                foreach (BillOption bill in BillOptions)
                {
                    db.BillOptions.Remove(bill);
                }
                db.Wallets.Remove(wallet);
            }

            db.Forums.Remove(ForumToDelete);
            db.SaveChanges();

            return new JsonResult(Message.GetMessage("S'ha eliminat el forum correctament."));
        }

        public bool ValidateUser(Forum forum)
        {
            User user = null;
            try
            {
                user = userGetter.GetUser();
                return (forum.Owner.Id == user.Id) ? true : false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Forum UpdateForum(Forum ForumToEdit, Forum forum)
        {
            if (forum.Title != null)
            {
                if (!(forum.Title.Length > 100))
                {
                    ForumToEdit.Title = forum.Title;
                }
            }
            if (forum.Description != null)
            {
                if (!(forum.Description.Length > 1000))
                {
                    ForumToEdit.Description = forum.Description;
                }
            }
            if (forum.Icon != null)
            {
                ForumToEdit.Icon = forum.Icon;
            }
            if (forum.Icon != null)
            {
                ForumToEdit.Background = forum.Background;
            }
            if (forum.DateBeginsVote != null)
            {
                ForumToEdit.DateBeginsVote = forum.DateBeginsVote;
            }
            if (forum.DateEndsVote != null)
            {
                ForumToEdit.DateEndsVote = forum.DateEndsVote;
            }
            return ForumToEdit;
        }

        public void GetTransactions(Forum forum)
        {
            db.Transactions.Add(new Transaction
            {
                Forum = forum,
                Name = "20",
                Value = 20
            });

            db.Transactions.Add(new Transaction
            {
                Forum = forum,
                Name = "50",
                Value = 50
            });

            db.Transactions.Add(new Transaction
            {
                Forum = forum,
                Name = "100",
                Value = 100
            });

            db.SaveChanges();
        }
    }
}
