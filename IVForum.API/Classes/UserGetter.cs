﻿using IVForum.API.Data;
using IVForum.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
                var userId = claimsPrincipal.Claims.Single(c => c.Type == "id");
                user = db.DbUsers.Where(c => c.IdentityId == userId.Value).FirstOrDefault();
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
                user = db.DbUsers.Where(c => c.IdentityId == userid).Include(x => x.Identity).FirstOrDefault();
                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
