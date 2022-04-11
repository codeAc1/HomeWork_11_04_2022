using FrontToBack.DAL;
using FrontToBack.Extensions;
using FrontToBack.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
        public IActionResult Create(Slider slider)
        {
            if (ModelState["Photo"].ValidationState== ModelValidationState.Invalid) return View();

            if (!slider.Photo.IsValidType("image/"))
            {
                ModelState.AddModelError("Photo", "Ancaq şəkil seçilə bilər");
                return View();
            }
            
            var size = 200;
            if (!slider.Photo.IsValidSize(size))
            {
                ModelState.AddModelError("Photo", $"Şəklin ölşüsü {size}-kb dan çox olmamalıdır sizin seçdiyiniz fayil {(Math.Ceiling((decimal)slider.Photo.Length)/1024).ToString("N2")} kb-dir");
                return View();
            }
            string root = Path.Combine(_env.WebRootPath,"img");
            return Content(root+" "+slider.Photo.FileName);
            using(FileStream fileStream = new FileStream(@"C:\Users\Admin\Desktop\Backend dersler\Serbest\04.04.2022\FrontToBack\FrontToBack\wwwroot\img\demo.png", FileMode.Create))
            {
                slider.Photo.CopyToAsync(fileStream);
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}
