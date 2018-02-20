using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IVForum.API.Data;
using IVForum.API.Models;
using IVForum.API.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IVForum.API.Controllers
{
    [Route("api/accounts")]
    public class AccountsController : Controller
    {
        private readonly DbHandler db;
        private readonly UserManager<UserModel> userManager;

        public AccountsController(UserManager<UserModel> _userManager, DbHandler _db)
        {
            db = _db;
            userManager = _userManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserModel userModel = new UserModel
            {
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                UserName = model.Email
            };

            User user = new User
            {
                Id = Guid.NewGuid(),
                Identity = userModel
            };

            var result = await userManager.CreateAsync(userModel, model.Password);

            var jsonResult = new
            {
                Code = 400,
                Status = "error"
            };

            if (!result.Succeeded) return new JsonResult(jsonResult);

            await db.DbUsers.AddAsync(user);
            await db.SaveChangesAsync();

            jsonResult = new 
            {
                Code = 200,
                Status = "correct"
            };

            return new JsonResult(jsonResult);
        }
    }
}