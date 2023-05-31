using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieScribe.Data;
using MovieScribe.Data.Services;
using MovieScribe.Models;

namespace MovieScribe.Controllers
{
    [Authorize(Roles = "admin, moderator")]
    public class WriterController : Controller
    {
        private readonly IWritersService _service;
        public WriterController(IWritersService service)
        {
            _service = service;
        }

        private async Task ProcessImageUploadAsync(IFormFile ImageUpload, WriterModel writer)
        {
            if (ImageUpload != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await ImageUpload.CopyToAsync(memoryStream);
                    writer.ImageData = memoryStream.ToArray();
                    writer.ImageMimeType = ImageUpload.ContentType;
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
        public async Task<IActionResult> Create([Bind("Name,Middle_Name,Surname,Age,Biography")] WriterModel writer, IFormFile ImageUpload)
        {
            await ProcessImageUploadAsync(ImageUpload, writer);

            if (!ModelState.IsValid)
            {
                return View(writer);
            }

            await _service.AddAsync(writer);
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var writerDetails = await _service.GetByIdAsync(id);
            if (writerDetails == null)
            {
                return View("NotFound");
            }
            return View(writerDetails);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var writerDetails = await _service.GetByIdAsync(id);
            if (writerDetails == null)
            {
                return View("NotFound");
            }
            return View(writerDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Middle_Name,Surname,Age,Biography,ImageData,ImageMimeType")] WriterModel writer, IFormFile ImageUpload)
        {
            var existingWriter = await _service.GetByIdAsync(id);
            if (existingWriter == null)
            {
                return View("NotFound");
            }

            existingWriter.Name = writer.Name;
            existingWriter.Middle_Name = writer.Middle_Name;
            existingWriter.Surname = writer.Surname;
            existingWriter.Age = writer.Age;
            existingWriter.Biography = writer.Biography;

            await ProcessImageUploadAsync(ImageUpload, existingWriter);

            writer = existingWriter;

            if (!ModelState.IsValid)
            {
                return View(existingWriter);
            }

            await _service.UpdateAsync(id, existingWriter);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var writerDetails = await _service.GetByIdAsync(id);
            if (writerDetails == null)
            {
                return View("NotFound");
            }
            return View(writerDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteWriter(int id)
        {
            var writerDetails = await _service.GetByIdAsync(id);
            if (writerDetails == null)
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
