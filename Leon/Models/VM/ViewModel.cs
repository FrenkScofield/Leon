using Leon.Models.BLL;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leon.Models.VM
{
    public class ViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<Product> Products { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public IEnumerable<ProductCategory> ProductCategories  { get; set; }
        public IEnumerable<SelectListItem> CategoryName { get; set; }
        public IEnumerable<Slider> Sliders { get; set; }
        public Slider Slider { get; set; }

    }
}
