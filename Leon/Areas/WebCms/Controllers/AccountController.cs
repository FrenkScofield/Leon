using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Leon.Models.DAL;
using Leon.Models.VM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Leon.Areas.WebCms.Controllers
{
    [Area("WebCms")]
    //[Route("WebCms/")]
    //[Route("WebCms/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly MyContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(MyContext context,
                                 UserManager<IdentityUser> userManager,
                                 SignInManager<IdentityUser> signInManager,
                                 RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);
            IdentityUser user = await _userManager.FindByEmailAsync(loginViewModel.Email);

            if (user == null)
            {
                ModelState.AddModelError("Email", "Email or Password is invalid");
                return View(loginViewModel);
            }

            Microsoft.AspNetCore.Identity.SignInResult signInResult =
                  await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, true, true);
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("Email", "Email or password is invalid");
                return View(loginViewModel);
            }
            return RedirectToAction("Index", "AdminHome", new { area = "WebCms" });
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
