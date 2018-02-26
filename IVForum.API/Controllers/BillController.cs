using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IVForum.API.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IVForum.API.Controllers
{
    [Route("api/bill")]
    public class BillController : Controller
    {
        private readonly DbHandler db;

        public BillController(DbHandler _db)
        {
            db = _db;
        }

        
    }
}