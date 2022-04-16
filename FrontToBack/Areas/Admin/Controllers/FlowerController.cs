using FrontToBack.DAL;
using FrontToBack.Extensions;
using FrontToBack.Helpers;
using FrontToBack.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FlowerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public FlowerController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            List<Product> flowers = _context.Products.ToList();
            return View(flowers);
        }
        public  async Task<IActionResult>  Create()
        {
            ViewBag.Category = await _context.Categories.ToListAsync();
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {

            #region Single File Upload
            if (ModelState["Photo"].ValidationState == ModelValidationState.Invalid) return View();

            if (!product.Photo.IsValidType("image/"))
            {
                ModelState.AddModelError("Photo", "Ancaq şəkil seçilə bilər");
                return View();
            }

            var size = 200;
            if (!product.Photo.IsValidSize(size))
            {
                ModelState.AddModelError("Photo", $"Şəklin ölşüsü {size}-kb dan çox olmamalıdır sizin seçdiyiniz fayil {Math.Ceiling((decimal)product.Photo.Length) / 1024:N2} kb-dir");
                return View();
            }


            product.ImageUrl = await product.Photo.SaveFileAsync(_env.WebRootPath, "img");
            await _context.Products.AddAsync(product);
            product.IsDeleted = false;


            #endregion



            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult>Detail(int? id)
        {
            if (id == null) return BadRequest();

            Product product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted == false);

            if (product == null) return NotFound();

            return View(product);
        }
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null) return BadRequest();

            Product product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted==false);

            if (product == null) return NotFound();

            return View(product);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null) return BadRequest();
            //Slider slider1 = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            Product product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();
            
            //Helper.DeleteFile(_env.WebRootPath, "img", product.ImageUrl);
            //_context.Products.Remove(product);
            product.IsDeleted = true;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(int? id)
        {
            if (id == null) return BadRequest();
            //Slider slider1 = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            Product product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            //Helper.DeleteFile(_env.WebRootPath, "img", product.ImageUrl);
            //_context.Products.Remove(product);
            product.IsDeleted = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
