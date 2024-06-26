﻿using Leon.Models.BLL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leon.Models.DAL
{
    public class MyContext : IdentityDbContext<IdentityUser>
    {
        public MyContext(DbContextOptions<MyContext> options) : base(options) { }

     
        public DbSet<Slider> Sliders { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<File> Files  { get; set; }
        //public DbSet<AppUser> AppUsers { get; set; }

    }
}
