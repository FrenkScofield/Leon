using Leon.Models.BLL;
using Leon.Models.DAL;
using Leon.Models.VM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leon.Controllers
{
  

    public class MasinlarContoller : Controller
    {
        private readonly MyContext _context;

        public MasinlarContoller(MyContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            ViewModel viewModel = new ViewModel
            {
                Product = new Product(),
                Products = await _context.Products.Include(c => c.ProductCategory).ToListAsync(),
                ProductCategories = await _context.ProductCategories.ToListAsync(),
                
            };
            return View(viewModel);
        }
    }
}