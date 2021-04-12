using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Leon.Models.BLL;
using Leon.Models.DAL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Leon
{
    public class Startup
    {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();


            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.AccessDeniedPath = "/WebCms/Account/Login";
            //    options.LoginPath = "/WebCms/Account/Login";
            //});

            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                ////Password
                //options.Password.RequireDigit = true;
                //options.Password.RequireLowercase = true;
                //options.Password.RequiredLength = 9;
                //options.Password.RequiredUniqueChars = 3;
                //options.Password.RequireNonAlphanumeric = false;
                //options.Password.RequireUppercase = false;

                ////Lock out
                //options.Lockout.AllowedForNewUsers = true;
                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(5);
                //options.Lockout.MaxFailedAccessAttempts = 3;

                //User
                //options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<MyContext>()
                   .AddDefaultTokenProviders();

            services.AddDbContext<MyContext>(options =>
            {
                options.UseSqlServer(_configuration["ConnectionStrings:Default"]);
            });


            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.AccessDeniedPath = "/WebCms/Account/Login";
                options.LoginPath = "/WebCms/Account/Login";
                options.LogoutPath = "/WebCms/Account/Logout";

            });
            //services.AddDistributedMemoryCache();
            //services.AddSession(options =>
            //{
            //    options.IdleTimeout = TimeSpan.FromDays(1);
            //    options.Cookie.HttpOnly = true;
            //    options.Cookie.IsEssential = true;
            //});
          
         

        }



        //create admin role
        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            //initializing custom roles 
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            string roleName = "Admin";
            IdentityResult roleResult;

            //foreach (var roleName in roleNames)
            //{
            var roleExist = await RoleManager.RoleExistsAsync(roleName);
            // ensure that the role does not exist
            if (!roleExist)
            {
                //create the roles and seed them to the database: 
                roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
            }
            //}

            // find the user with the admin email 
            var _user = await UserManager.FindByEmailAsync("admin@gmail.com");

            // check if the user exists
            if (_user == null)
            {
                //Here you could create the super admin who will maintain the web app
                var poweruser = new IdentityUser
                {
                    UserName = "Admin",
                    Email = "admin@gmail.com",
                };
                string adminPassword = "Admin123@@";

                var createPowerUser = await UserManager.CreateAsync(poweruser, adminPassword);
                if (createPowerUser.Succeeded)
                {
                    //here we tie the new user to the role
                    await UserManager.AddToRoleAsync(poweruser, "Admin");

                }
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            // app.UseAuthentication();
           

            app.UseEndpoints(endpoints =>
            {
                
                endpoints.MapAreaControllerRoute(
                 name: "areas", "WebCms",
                 pattern: "{area:exists}/{controller=Account}/{action=Login}/{id?}");


                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{url?}");

                endpoints.MapControllerRoute(
               name: "default",
               pattern: "{controller=Home}/{action=Index}/{id?}");

            });


            Task.Run(() => CreateRoles(serviceProvider)).Wait();
        }
    }
}
