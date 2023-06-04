using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieScribe.Data;
using MovieScribe.Data.Services;
using MovieScribe.Data.Static;
using MovieScribe.Models;
using System.Data;
using static System.Net.Mime.MediaTypeNames;

namespace MovieScribe.Controllers
{
    // This is the ActorController class, responsible for handling HTTP requests related to actors. 
    // The [Authorize] attribute enforces that only authenticated users with "admin" or "moderator" roles can access the controller.
    [Authorize(Roles = "admin, moderator")]
    public class ActorController : Controller
    {
        // IActorsService is a service responsible for actor-related business logic. It's injected via the constructor.
        private readonly IActorsService _service;
        public ActorController(IActorsService service)
        {
            _service = service;
        }

        // This is a private helper method to process image uploads. It reads the file into a byte array which is stored in the ActorModel.
        private async Task ProcessImageUploadAsync(IFormFile ImageUpload, ActorModel actor)
        {
            if (ImageUpload != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await ImageUpload.CopyToAsync(memoryStream);
                    actor.ImageData = memoryStream.ToArray();
                    actor.ImageMimeType = ImageUpload.ContentType;
                }
            }
            else
            {
                ModelState.Remove("ImageUpload");
            }
        }

        // This is the action for the index view. It retrieves a list of all actors and sends them to the view.
        public async Task<IActionResult> Index()
        {
            var result = await _service.GetAllAsync();
            return View(result);
        }

        // This action returns the "Create" view, which allows the user to create a new actor.
        public IActionResult Create()
        {
            return View();
        }

        // This action processes a POST request from the "Create" form. If the form is valid, it creates a new actor.
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name,Middle_Name,Surname,Age,Biography")] ActorModel actor, IFormFile ImageUpload)
        {
            await ProcessImageUploadAsync(ImageUpload, actor);

            if (!ModelState.IsValid)
            {
                return View(actor);
            }

            await _service.AddAsync(actor);
            return RedirectToAction(nameof(Index));
        }

        // This action returns the "Details" view, which displays detailed information about a specific actor. 
        // It allows anonymous access, so even unauthenticated users can view actor details.
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var actorDetails = await _service.GetByIdAsync(id);
            if (actorDetails == null)
            {
                return View("NotFound");
            }
            return View(actorDetails);
        }

        // This action returns the "Edit" view for a specific actor, which allows the user to update the actor's information.
        public async Task<IActionResult> Edit(int id)
        {
            var actorDetails = await _service.GetByIdAsync(id);
            if (actorDetails == null)
            {
                return View("NotFound");
            }
            return View(actorDetails);
        }

        // This action processes a POST request from the "Edit" form. If the form is valid, it updates the actor's information.
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Middle_Name,Surname,Age,Biography,ImageData,ImageMimeType")] ActorModel actor, IFormFile ImageUpload)
        {
            if (id <= 0 || actor == null)
            {
                return NotFound();
            }

            var existingActor = await _service.GetByIdAsync(id);
            if (existingActor == null)
            {
                return NotFound();
            }

            existingActor.Name = actor.Name;
            existingActor.Middle_Name = actor.Middle_Name;
            existingActor.Surname = actor.Surname;
            existingActor.Age = actor.Age;
            existingActor.Biography = actor.Biography;

            await ProcessImageUploadAsync(ImageUpload, existingActor);

            actor = existingActor;

            if (!ModelState.IsValid)
            {
                return View(existingActor);
            }

            await _service.UpdateAsync(id, existingActor);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actorDetails = await _service.GetByIdAsync(id);
            if (actorDetails == null)
            {
                return View("NotFound");
            }

            await _service.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

        // This action handles searches, returning a list of actors that match the search query. It allows anonymous access.
        [AllowAnonymous]
        public async Task<IActionResult> Search(string query)
        {
            var results = await _service.Search(query);
            return View("Index", results);
        }

    }
}
