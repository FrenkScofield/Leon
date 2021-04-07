using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leon.Models.BLL
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Company { get; set; }
        public string ImageCode { get; set; }
        public int Lifting { get; set; }
        public int Height { get; set; }
        public int Length { get; set; }
        public string  OperatingMode { get; set; }
        public DateTime DateOfProduction { get; set; }

        //foreign key connection
        public int ProductCategoryId { get; set; }

        public virtual ProductCategory ProductCategory { get; set; }
    }
}
