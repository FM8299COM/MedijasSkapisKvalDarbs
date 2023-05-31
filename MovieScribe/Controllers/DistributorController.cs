using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieScribe.Data;
using MovieScribe.Data.Services;
using MovieScribe.Models;
using System.Data;

namespace MovieScribe.Controllers
{
    [Authorize(Roles = "admin, moderator")]
    public class DistributorController : Controller
    {
        private readonly IDistributorService _service;
        private readonly ILogger<DistributorController> _logger;
        public DistributorController(IDistributorService service, ILogger<DistributorController> logger)
        {
            _service = service;
            _logger = logger;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var result = await _service.GetAllAsync();
            return View(result);
        }

        private void LogModelErrors()
        {
            var errors = ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .Select(e => new { Name = e.Key, Message = e.Value.Errors.First().ErrorMessage })
                .ToList();

            foreach (var error in errors)
            {
                _logger.LogInformation($"Error in property {error.Name}: {error.Message}");
            }
        }
        private async Task ProcessImageUploadAsync(IFormFile ImageUpload, DistributorModel distributor)
        {
            if (ImageUpload != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await ImageUpload.CopyToAsync(memoryStream);
                    distributor.ImageData = memoryStream.ToArray();
                    distributor.ImageMimeType = ImageUpload.ContentType;
                }
            }
            else
            {
                ModelState.Remove("ImageUpload");
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name,Description")] DistributorModel distributor, IFormFile ImageUpload)
        {
            await ProcessImageUploadAsync(ImageUpload, distributor);

            if (!ModelState.IsValid)
            {
                LogModelErrors();
                return View(distributor);
            }

            await _service.AddAsync(distributor);
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var distributorDetails = await _service.GetByIdAsync(id);
            if (distributorDetails == null)
            {
                return View("NotFound");
            }
            return View(distributorDetails);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var distributorDetails = await _service.GetByIdAsync(id);
            if (distributorDetails == null)
            {
                return View("NotFound");
            }
            return View(distributorDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Description")] DistributorModel distributor, IFormFile ImageUpload)
        {
            if (id <= 0 || distributor == null)
            {
                return NotFound();
            }

            var existingDistributor = await _service.GetByIdAsync(id);
            if (existingDistributor == null)
            {
                return NotFound();
            }

            existingDistributor.Name = distributor.Name;
            existingDistributor.Description = distributor.Description;

            await ProcessImageUploadAsync(ImageUpload, existingDistributor);

            if (!ModelState.IsValid)
            {
                LogModelErrors();
                return View(existingDistributor);
            }

            await _service.UpdateAsync(id, existingDistributor);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var distributorDetails = await _service.GetByIdAsync(id);
            if (distributorDetails == null)
            {
                return View("NotFound");
            }
            return View(distributorDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteDistributor(int id)
        {
            var distributorDetails = await _service.GetByIdAsync(id);
            if (distributorDetails == null)
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
