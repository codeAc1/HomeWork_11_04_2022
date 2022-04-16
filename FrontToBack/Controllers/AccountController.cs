using FrontToBack.DAL;
using FrontToBack.Helpers;
using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController
            (
                UserManager<AppUser> userManager, 
                SignInManager<AppUser> signInManager, 
                AppDbContext context, 
                RoleManager<IdentityRole> roleManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _roleManager = roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = await _userManager.FindByNameAsync(login.UserName);
            if (user == null)
            {
                ModelState.AddModelError("","Username or Password is wrong");
                return View();
            }
            if (user.IsDeleted)
            {
                ModelState.AddModelError("", "User Deactived");
                return View();
            }
            var signInResult= await _signInManager.PasswordSignInAsync(user, login.Password, login.RememberMe,true);

            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", "User is blocket wait 5 minutes");
                return View();
            }

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Username or Password is wrong");
                return View();
            }

            //return Json("ok");

            //user.IsOnline = true;
            //await _context.SaveChangesAsync();

            if ((await _userManager.GetRolesAsync(user))[0]==Roles.Admin.ToString())
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult>Register(RegisterVM register)
        {
            if (!ModelState.IsValid) return View();

            AppUser newUser = new AppUser
            {
                Name = register.Name,
                SurName=register.SurName,
                UserName=register.UserName,
                Email=register.Email
                
            };

            IdentityResult identityResult= await _userManager.CreateAsync(newUser,register.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
                
            }
            //await _userManager.AddToRoleAsync(newUser,Roles.Admin.ToString()); admin ucun
            await _userManager.AddToRoleAsync(newUser, Roles.Member.ToString());
            await _signInManager.SignInAsync(newUser,true);
            
            //if (User.Identity.IsAuthenticated)
            //{
            //    newUser.IsOnline = true;
            //}
            //await _context.SaveChangesAsync();


            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        #region Create Role
        //public async Task CreateRoles()
        //{
        //    foreach (var role in Enum.GetValues(typeof(Roles)))
        //    {
        //        if (!(await _roleManager.RoleExistsAsync(role.ToString())))
        //        {
        //            await _roleManager.CreateAsync(new IdentityRole { Name = role.ToString() });
        //        }
        //    }
        //}
        #endregion
    }
}
