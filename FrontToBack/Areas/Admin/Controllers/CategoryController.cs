using FrontToBack.DAL;
using FrontToBack.Models;
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
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            
            List<Category> categories = await _context.Categories.Where(c=>!c.IsDeleted).OrderByDescending(c=>c.Id).ToListAsync();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (!ModelState.IsValid) return View();
            bool isExist = _context.Categories.Any(ct => ct.Name.ToLower().Trim()== (category.Name).ToLower().Trim());
            
            if (isExist)
            {
                ModelState.AddModelError("Name",$"{category.Name}-adlı categoriya artıq mövcuddur");
                return View();
            }
            
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category == null) return NotFound();
            return View(category);
        }
        public async Task<IActionResult>Delete(int? id)
        {
            if (id == null) return BadRequest();
            Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category == null) return NotFound();
            return View(category);



        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null) return BadRequest();
            Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (category == null) return NotFound();
            category.IsDeleted = true;
             //_context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (category == null) return NotFound();
            return View(category);



        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Update")]
        public async Task<IActionResult> UpdateCategory(int? id, Category category)
        {
            if (id == null) return BadRequest();
            Category categoryView = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) return NotFound();
            if (!ModelState.IsValid)
            {
                return View(categoryView);
            }
            Category categoryDb = await _context.Categories.FirstOrDefaultAsync(c =>c.Name.ToLower().Trim()==category.Name.ToLower().Trim());
            if (categoryDb != null && categoryDb.Id!=id)
            {
                ModelState.AddModelError("Name", $"{category.Name}-adli Kategoriya var");
                return View(categoryView);
            }

            categoryView.Name = category.Name;
            categoryView.Description = category.Description;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }
    }
}
