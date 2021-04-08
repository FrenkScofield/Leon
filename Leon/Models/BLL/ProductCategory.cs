using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leon.Models.BLL
{
    public class ProductCategory
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string Url { get; set; }
        public string ImageCode { get; set; }

        //primary key connection
        public virtual ICollection<Product> Products { get; set; }
    }
}
