using Leon.Models.BLL;
using Leon.Models.DAL;
using Leon.Models.Extensiyon;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Leon.Areas.WebCms.Controllers
{
    [Area("WebCms")]
    // [Route("WebCms/")]
    [Route("WebCms/[controller]/[action]")]

    public class SliderController : Controller
    {
        private readonly MyContext _context;
        public readonly IWebHostEnvironment _env;

        public SliderController(MyContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        //Slider index Function Start
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sliders.ToListAsync());
        }
        //Slider index Function End

        //Slider Create Function Start
        public IActionResult Create()
        {
            return View();
        }

        //Post section

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile file, Slider slider)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    slider.ImageCode = await ImagesHelpers.ImageUploadAsync(_env.WebRootPath, file, "img","slider");
                }
                 _context.Add(slider);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(slider);
        }
        //Slider Create Function End

        //Slider Edit Function Start
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null)
            {
                return NotFound();
            }
            return View(slider);
        }

        //Post section

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit( IFormFile file, Slider slider)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null)
                    {
                        if (slider.ImageCode != null && slider.ImageCode != string.Empty)
                        {
                            ImagesHelpers.DeleteImage(slider.ImageCode, "img/slider/");
                        }
                        slider.ImageCode = await ImagesHelpers.ImageUploadAsync(_env.WebRootPath, file, "img", "slider");

                    }
                    _context.Update(slider);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SliderExists(slider.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(slider);
        }

        private bool SliderExists(int id)
        {
            return _context.Sliders.Any(e => e.Id == id);
        }
        //Slider Edit Function End

        //Slider Delete Function Start
        public async Task<IActionResult> Delete(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider.ImageCode != null && slider.ImageCode != string.Empty)
            {
                ImagesHelpers.DeleteImage(slider.ImageCode, "img/slider/");
            }
            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //Slider Delete Function End


    }
}
    

