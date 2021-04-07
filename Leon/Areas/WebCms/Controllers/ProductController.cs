using Leon.Models.BLL;
using Leon.Models.DAL;
using Leon.Models.Extensiyon;
using Leon.Models.VM;
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

        //Model Create Function Start
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
            if (!ModelState.IsValid)
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
        //Model Create Function End




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


        //public async Task<IActionResult> Edit(int? Id)
        //{
        //    if (Id == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewModel viewModel = new ViewModel();

        //    viewModel.Clinics = await _context.Clinics.ToListAsync();

        //    viewModel.Departments = await _context.Departments.Include(t => t.DepartmentTranslates).ToListAsync();

        //    viewModel.Files = await _context.Files.ToListAsync();

        //    viewModel.Clinic_Select = new SelectList(await _context.Clinics.ToListAsync(), "Id", "Name");

        //    viewModel.Department_Select = new SelectList(await _context.Departments.Include(t => t.DepartmentTranslates).ToListAsync(), "Id", "Name");

        //    viewModel.ClinicDoctor = await _context.ClinicDoctors.Include(t => t.ClinicDoctorTranslates).Include(c => c.Clinic)
        //                                                         .Include(d => d.Department)
        //                                                         .FirstOrDefaultAsync(a => a.Id == Id);

        //    viewModel.ClinicDoctorTranslates = viewModel.ClinicDoctor.ClinicDoctorTranslates;

        //    return View(viewModel);
        //}

        ////Post part
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(IFormFile file, ClinicDoctor clinicDoctor, List<ClinicDoctorTranslate> translates)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            if (file != null)
        //            {
        //                if (ImagesHelpers.ImageIsValid(file))
        //                {
        //                    if (clinicDoctor.ImageCode != null && clinicDoctor.ImageCode != string.Empty)
        //                    {
        //                        ImagesHelpers.DeleteImage(clinicDoctor.ImageCode, "Images/clinicDoctor/");
        //                    }
        //                    clinicDoctor.ImageCode = await _upload.Edit(file, "Images/clinicDoctor/", null, clinicDoctor.ImageCode);
        //                }
        //            }

        //            _context.Update(clinicDoctor);
        //            foreach (var item in translates)
        //            {
        //                _context.ClinicDoctorTranslates.Update(item);
        //                await _context.SaveChangesAsync();
        //            }

        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!ClinicExists(clinicDoctor.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(clinicDoctor);
        //}

        //private bool ClinicExists(int id)
        //{
        //    return _context.ClinicDoctors.Any(e => e.Id == id);
        //}
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
