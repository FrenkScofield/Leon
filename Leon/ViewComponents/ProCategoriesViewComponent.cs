using Leon.Models.DAL;
using Leon.Models.VM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leon.ViewComponents
{
    public class ProCategoriesViewComponent : ViewComponent
    {
        private readonly MyContext _context;
        public ProCategoriesViewComponent(MyContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var proCategories = await _context.ProductCategories.ToListAsync();

            ViewModel model = new ViewModel()
            {
                ProductCategories = proCategories
            };

            //var model = new 
            //{
            //    Categories = await _context.Categories.ToListAsync(),
            //    SubCategories = await _context.SubCategories.ToListAsync()
            //};

            return await Task.FromResult((IViewComponentResult)View("ProCategories", model));
        }
    }
}
