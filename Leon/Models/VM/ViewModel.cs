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
        public File File{ get; set; }
        public IEnumerable<File> Files { get; set; }

        public int Category { get; set; }

        public IEnumerable<SelectListItem> CategoryName { get; set; }
    }
}
