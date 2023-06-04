using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieScribe.Data;
using MovieScribe.Data.Services;
using MovieScribe.Models;
using System.IO;

namespace MovieScribe.Controllers
{
    [Authorize(Roles = "admin, moderator")]
    public class StudioController : Controller
    {
        private readonly IStudiosService _service;
        public StudioController(IStudiosService service)
        {
            _service = service;
        }

        private async Task ProcessImageUploadAsync(IFormFile ImageUpload, StudioModel studio)
        {
            if (ImageUpload != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await ImageUpload.CopyToAsync(memoryStream);
                    studio.ImageData = memoryStream.ToArray();
                    studio.ImageMimeType = ImageUpload.ContentType;
                }
            }
            else
            {
                ModelState.Remove("ImageUpload");
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var result = await _service.GetAllAsync();
            return View(result);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name,Description")] StudioModel studio, IFormFile ImageUpload)
        {
            await ProcessImageUploadAsync(ImageUpload, studio);

            if (!ModelState.IsValid)
            {
                return View(studio);
            }

            await _service.AddAsync(studio);
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var studioDetails = await _service.GetByIdAsync(id);
            if (studioDetails == null)
            {
                return View("NotFound");
            }
            return View(studioDetails);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var studioDetails = await _service.GetByIdAsync(id);
            if (studioDetails == null)
            {
                return View("NotFound");
            }
            return View(studioDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description,ImageData,ImageMimeType")] StudioModel studio, IFormFile ImageUpload)
        {
            var existingStudio = await _service.GetByIdAsync(id);
            if (existingStudio == null)
            {
                return View("NotFound");
            }

            existingStudio.Name = studio.Name;
            existingStudio.Description = studio.Description;

            await ProcessImageUploadAsync(ImageUpload, existingStudio);

            studio = existingStudio;

            if (!ModelState.IsValid)
            {
                return View(existingStudio);
            }

            await _service.UpdateAsync(id, existingStudio);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studioDetails = await _service.GetByIdAsync(id);
            if (studioDetails == null)
            {
                return View("NotFound");
            }

            await _service.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Search(string query)
        {
            var results = await _service.Search(query);
            return View("Index", results);
        }
    }
}
