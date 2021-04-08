using Leon.Models.BLL;
using Leon.Models.DAL;
using Leon.Models.Extensiyon;
using Leon.Models.VM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leon.Areas.WebCms.Controllers
{
    [Area("WebCms")]
    // [Route("WebCms/")]
    [Authorize(Roles = "Admin")]

    [Route("WebCms/[controller]/[action]")]
    public class ProductController : Controller
    {
        private readonly MyContext _context;
        public readonly IWebHostEnvironment _env;


        public ProductController(MyContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;

        }
        public async Task<IActionResult> Index()
        {
            var spesficData = _context.Products.Include(c => c.ProductCategory);
            ViewModel viewModel = new ViewModel
            {
                Products = await _context.Products.Include(d => d.ProductCategory).ToListAsync(),

                //    Files = await _context.Files.ToListAsync()
            };

            return View(viewModel);
        }

        // Product Create Function Start
        public IActionResult Create()
        {
            ViewModel vm = new ViewModel()
            {
                CategoryName = _context.ProductCategories.Select(c => new SelectListItem() { Text = c.CategoryName, Value = c.Id.ToString() }),
                Product = new Product()
            };
            return View(vm);
        }
        //Post section
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ViewModel viewModel, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var Catagory = await _context.ProductCategories.FindAsync(viewModel.Product.ProductCategoryId);
                Product model = new Product()
                {
                    Title = viewModel.Product.Title,
                    Description = viewModel.Product.Description,
                    Company = viewModel.Product.Company,
                    Lifting = viewModel.Product.Lifting,
                    Height = viewModel.Product.Height,
                    Length = viewModel.Product.Length,
                    OperatingMode = viewModel.Product.OperatingMode,
                    DateOfProduction = viewModel.Product.DateOfProduction,
                    ProductCategoryId = viewModel.ProductCategory.Id,
                    ImageCode = await ImagesHelpers.ImageUploadAsync(_env.WebRootPath, file, "img", "category"),
                    ProductCategory = Catagory

                };
                await _context.Products.AddAsync(model);

                await _context.SaveChangesAsync();

            }
            return RedirectToAction(nameof(Index));
        }
        // Product Create Function End




        //Edit Function Start
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }
            var model = await _context.Products.FindAsync(Id);

            ViewModel vm = new ViewModel();

            vm.ProductCategories = await _context.ProductCategories.ToListAsync();

            vm.CategoryName = _context.ProductCategories.Select(c => new SelectListItem() { Text = c.CategoryName, Value = c.Id.ToString(), Selected = c.Id == model.ProductCategoryId });

            vm.Product = model;


            return View(vm);
        }

        //Post part
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IFormFile file,ViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var DataProdatc = await _context.Products.FindAsync(viewModel.Product.Id);

                if (file != null)
                {
                    if (ImagesHelpers.ImageIsValid(file))
                    {
                        if (DataProdatc.ImageCode != null && DataProdatc.ImageCode != string.Empty)
                        {
                            ImagesHelpers.DeleteImage(DataProdatc.ImageCode, "img/imgProduct/");
                        }
                        DataProdatc.ImageCode = await ImagesHelpers.ImageUploadAsync(_env.WebRootPath, file, "img", "category");
                    }
                }

                await _context.SaveChangesAsync();

                DataProdatc.Title = viewModel.Product.Title;
                    DataProdatc.Description = viewModel.Product.Description;
                    DataProdatc.Company = viewModel.Product.Company;
                    DataProdatc.Lifting = viewModel.Product.Lifting;
                    DataProdatc.Height = viewModel.Product.Height;
                DataProdatc.Length = viewModel.Product.Length;
                    DataProdatc.OperatingMode = viewModel.Product.OperatingMode;
                    DataProdatc.DateOfProduction = viewModel.Product.DateOfProduction;
                    DataProdatc.ProductCategoryId = viewModel.ProductCategory.Id;
                   


                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        private bool ClinicExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
        //Edit Function End

        // Delete Function Start
        public async Task<IActionResult> Delete(int id)
        {
            var clinicDoctor = await _context.Products.FindAsync(id);
            if (clinicDoctor.ImageCode != null && clinicDoctor.ImageCode != string.Empty)
            {
                ImagesHelpers.DeleteImage(clinicDoctor.ImageCode, "Images/clinicDoctor/");
            }
            _context.Products.Remove(clinicDoctor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // Delete Function End
    }
}
