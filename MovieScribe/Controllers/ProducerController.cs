using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieScribe.Data;
using MovieScribe.Data.Services;
using MovieScribe.Models;
using static System.Net.Mime.MediaTypeNames;


namespace MovieScribe.Controllers
{
    [Authorize(Roles = "admin, moderator")]
    public class ProducerController : Controller
    {
        private readonly IProducerService _service;
        public ProducerController(IProducerService service)
        {
            _service = service;
        }

        private async Task ProcessImageUploadAsync(IFormFile ImageUpload, ProducerModel producer)
        {
            if (ImageUpload != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await ImageUpload.CopyToAsync(memoryStream);
                    producer.ImageData = memoryStream.ToArray();
                    producer.ImageMimeType = ImageUpload.ContentType;
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
        public async Task<IActionResult> Create([Bind("Name,Middle_Name,Surname,Age,Biography")] ProducerModel producer, IFormFile ImageUpload)
        {
            await ProcessImageUploadAsync(ImageUpload, producer);

            if (!ModelState.IsValid)
            {
                return View(producer);
            }

            await _service.AddAsync(producer);
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var producerDetails = await _service.GetByIdAsync(id);
            if (producerDetails == null)
            {
                return View("NotFound");
            }
            return View(producerDetails);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var producerDetails = await _service.GetByIdAsync(id);
            if (producerDetails == null)
            {
                return View("NotFound");
            }
            return View(producerDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Middle_Name,Surname,Age,Biography,ImageData,ImageMimeType")] ProducerModel producer, IFormFile ImageUpload)
        {
            var existingProducer = await _service.GetByIdAsync(id);
            if (existingProducer == null)
            {
                return View("NotFound");
            }

            existingProducer.Name = producer.Name;
            existingProducer.Middle_Name = producer.Middle_Name;
            existingProducer.Surname = producer.Surname;
            existingProducer.Age = producer.Age;
            existingProducer.Biography = producer.Biography;

            await ProcessImageUploadAsync(ImageUpload, existingProducer);

            producer = existingProducer;

            if (!ModelState.IsValid)
            {
                return View(existingProducer);
            }

            await _service.UpdateAsync(id, existingProducer);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producerDetails = await _service.GetByIdAsync(id);
            if (producerDetails == null)
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
