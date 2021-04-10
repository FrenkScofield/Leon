using Leon.Models.BLL;
using Leon.Models.DAL;
using Leon.Models.Extensiyon;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leon.Areas.WebCms.Controllers
{
    [Area("WebCms")]
    // [Route("WebCms/")]
    [Route("WebCms/[controller]/[action]")]
   // [Authorize(Roles = "Admin")]

    public class ProductCategoryController : Controller
    {
        private readonly MyContext _context;
        public readonly IWebHostEnvironment _env;


        public ProductCategoryController(MyContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        //ProductCategory Index Function Start
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductCategories.ToListAsync());
        }
        //ProductCategory Index Function End


        //ProductCategory Create Function Start
        public IActionResult Create()
        {
            return View();
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( IFormFile file, ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    productCategory.ImageCode = await ImagesHelpers.ImageUploadAsync(_env.WebRootPath, file, "img", "category");
                }

                var result = await _context.ProductCategories.AddAsync(productCategory);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        //ProductCategory Create Function End

        //ProductCategory Edit Function Start
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var Brand = await _context.ProductCategories.FirstOrDefaultAsync(x => x.Id == id);

            if (Brand == null)
            {
                return NotFound();
            }
            return View(Brand);
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormFile file, int id, ProductCategory category )
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(category);
            }
            if (file != null)
            {
                if (category.ImageCode != null && category.ImageCode != string.Empty)
                {
                    ImagesHelpers.DeleteImage(category.ImageCode, "img/category/");
                }
                category.ImageCode = await ImagesHelpers.ImageUploadAsync(_env.WebRootPath, file, "img", "category");

            }
            _context.Update(category);
            await _context.SaveChangesAsync();

            var ProCategory = await _context.ProductCategories.FindAsync(id);

            ProCategory.CategoryName = category.CategoryName;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        //ProductCategory Edit Function End

        //ProductCategory Delete Function Start
        public async Task<IActionResult> Delete(int id)
        {
            var ProBrand = await _context.ProductCategories.FindAsync(id);
            _context.ProductCategories.Remove(ProBrand);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //ProductCategory Delete Function End
    }
}
