using FrontToBack.DAL;
using FrontToBack.Models;
using FrontToBack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM
            {
                Sliders = _context.Sliders.ToList(),
                Captian = _context.Captians.FirstOrDefault(),
                Categories=_context.Categories.ToList(),
                Products=_context.Products.Include(p=>p.Category).OrderByDescending(p=>p.Id).Take(8).ToList(),
                About=_context.About.FirstOrDefault()
            };
            return View(homeVM);
        }

        
    }
}
