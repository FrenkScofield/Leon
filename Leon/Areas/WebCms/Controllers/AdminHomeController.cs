using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leon.Areas.WebCms.Controllers
{
    [Area("WebCms")]
    //[Route("WebCms/")]
    //[Authorize]
    [Route("WebCms/[controller]/[action]")]
    public class AdminHomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
