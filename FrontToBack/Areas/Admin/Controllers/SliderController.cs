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
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FrontToBack.Helpers;

namespace FrontToBack.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            return View(_context.Sliders.ToList());
        }
        public  IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>  Create(Slider slider)
        {
            if (ModelState["Photo"].ValidationState == ModelValidationState.Invalid) return View();

            if (!slider.Photo.IsValidType("image/"))
            {
                ModelState.AddModelError("Photo", "Ancaq şəkil seçilə bilər");
                return View();
            }

            var size = 200;
            if (!slider.Photo.IsValidSize(size))
            {
                ModelState.AddModelError("Photo", $"Şəklin ölşüsü {size}-kb dan çox olmamalıdır sizin seçdiyiniz fayil {(Math.Ceiling((decimal)slider.Photo.Length) / 1024).ToString("N2")} kb-dir");
                return View();
            }
            //string space = "-";
            //string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff")+space+Guid.NewGuid().ToString()+ space + slider.Photo.FileName;
            //string root = Path.Combine(_env.WebRootPath,"img");
            //string resultPath = Path.Combine(root, fileName);


            //using (FileStream fileStream = new FileStream(resultPath, FileMode.Create))
            //{
            //    await slider.Photo.CopyToAsync(fileStream);
            //}

            slider.ImageUrl = await slider.Photo.SaveFileAsync(_env.WebRootPath, "img");
            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            //Slider slider1 = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            Slider slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return NotFound();
            return View(slider);
            


        }
        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            if (id == null) return BadRequest();
            //Slider slider1 = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            Slider slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return NotFound();
            Helper.DeleteFile(_env.WebRootPath, "img", slider.ImageUrl);
            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
