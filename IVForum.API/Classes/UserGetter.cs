using IVForum.API.Data;
using IVForum.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IVForum.API.Classes
{
    public class UserGetter
    {
        private readonly ClaimsPrincipal claimsPrincipal;
        private readonly DbHandler db;

        public UserGetter(DbHandler _db, IHttpContextAccessor httpContextAccessor)
        {
            db = _db;
            claimsPrincipal = httpContextAccessor.HttpContext.User;
        }

        public User GetUser()
        {
            User user = null;
            try
            {
                var userId = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "id");
                if (userId is null)
                {
                    Debug.WriteLine(claimsPrincipal);
                }
                user = db.DbUsers.Where(c => c.Identity.Id == userId.Value).FirstOrDefault();
                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public User GetUser(string userid)
        {
            User user = null;
            try
            {
                user = db.DbUsers.Where(c => c.Id.ToString() == userid).FirstOrDefault();
                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
