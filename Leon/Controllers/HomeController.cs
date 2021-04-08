using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Leon.Models;
using Leon.Models.DAL;
using Leon.Models.VM;
using Leon.Models.BLL;
using Microsoft.EntityFrameworkCore;

namespace Leon.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly MyContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, MyContext context)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            ViewModel viewModel = new ViewModel
            {
                Product = new Product(),
                Products = await _context.Products.Include(c => c.ProductCategory).ToListAsync(),
                ProductCategories = await _context.ProductCategories.ToListAsync(),
                Sliders = await _context.Sliders.ToListAsync()
            }; 
            return View(viewModel);
        }

        public async Task<IActionResult> Product(int productCategoryId)
        {
            ViewModel viewModel = new ViewModel
            {
                ProductCategories = await _context.ProductCategories.ToListAsync(),
                Products = await _context.Products.Where(p => p.ProductCategoryId == productCategoryId)
                                                  .ToListAsync(),
                ShowOnlyCategories = productCategoryId == 0
            };
            
            return View(viewModel);
        }

        public IActionResult OurProjects()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult About()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
