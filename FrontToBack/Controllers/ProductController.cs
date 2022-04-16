using FrontToBack.DAL;
using FrontToBack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            ViewBag.ProductCount = _context.Products.Count();
            //return View(_context.Products.Include(p => p.Category).OrderByDescending(p=>p.Id).Take(8).ToList());
            return View();
        }

        public IActionResult LoadMore(int skip)
        {
            List<Product> model = _context.Products.Include(p => p.Category).OrderByDescending(p => p.Id).Skip(skip).Take(8).ToList();
            return PartialView("_ProductPartial",model);
            //return Json(_context.Products.ToList());
        }
    }
}
