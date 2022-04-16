using FrontToBack.DAL;
using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _context;

        public UsersController(UserManager<AppUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult>  Index()
        {
            List<AppUser> users = _userManager.Users.ToList();
            List<UserVM> userVMs = new List<UserVM>();
            foreach (AppUser user in users)
            {
                UserVM userVM = new UserVM
                {
                    Id=user.Id,
                    Email=user.Email,
                    Username=user.UserName,
                    Name=user.Name,
                    SurName=user.SurName,
                    IsDeleted=user.IsDeleted,
                    Role=(await _userManager.GetRolesAsync(user))[0]
                };
                userVMs.Add(userVM);
            }
            //return Json(userVMs);
            return View(userVMs);
        }

        public async Task<IActionResult> Activated(string id)
        {
            
            


            return View();
        }
    }
}
