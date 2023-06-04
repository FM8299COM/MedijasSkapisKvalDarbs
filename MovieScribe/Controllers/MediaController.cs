using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MovieScribe.Data;
using MovieScribe.Data.Services;
using MovieScribe.Models;
using System.Data;
using System.Text;
using X.PagedList;

namespace MovieScribe.Controllers
{
    // MediaController class for managing media. The [Authorize] attribute is used to restrict access to users with "admin" or "moderator" roles.
    [Authorize]
    public class MediaController : Controller
    {
        // Instances of media service, user manager and database context are injected into the controller.
        private readonly IMediaService _service;
        private readonly UserManager<AppUser> _userManager;
        private readonly DBContext _context;

        // The constructor for the MediaController class, with required dependencies.
        public MediaController(IMediaService service, UserManager<AppUser> userManager, DBContext context)
        {
            _service = service;
            _userManager = userManager;
            _context = context;
        }

        // This method is for populating dropdown values for the media.
        private async Task PopulateDropdownValues()
        {
            var mediaDropdownData = await _service.GetMediaDropdownViewValues();

            ViewData["ActorIDs"] = new SelectList(mediaDropdownData.Actors, "ID", "FullName");
            ViewData["GenreIDs"] = new SelectList(mediaDropdownData.Genres, "ID", "Genre");
            ViewData["LanguageIDs"] = new SelectList(mediaDropdownData.Languages, "ID", "Language");
            ViewData["WriterIDs"] = new SelectList(mediaDropdownData.Writers, "ID", "FullName");
            ViewData["ProducerID"] = new SelectList(mediaDropdownData.Producers, "ID", "FullName");
            ViewData["DistributorID"] = new SelectList(mediaDropdownData.Distributors, "ID", "Name");
            ViewData["StudioID"] = new SelectList(mediaDropdownData.Studios, "ID", "Name");
        }

        // This method is for processing image uploads.
        private async Task ProcessImageUploadAsync(IFormFile ImageUpload, MediaViewModel media)
        {
            if (ImageUpload != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await ImageUpload.CopyToAsync(memoryStream);
                    media.ImageData = memoryStream.ToArray();
                    media.ImageMimeType = ImageUpload.ContentType;
                }
            }
            else
            {
                ModelState.Remove("ImageUpload");
            }
        }

        // This method maps the MediaModel to a MediaViewModel.
        private MediaViewModel MapToMediaViewModel(MediaModel media)
        {
            return new MediaViewModel
            {
                ID = media.ID,
                Title = media.Title,
                Runtime = media.Runtime,
                Description = media.Description,
                Aired = media.Aired,
                Type = media.Type,
                ProducerID = media.ProducerID,
                DistributorID = media.DistributorID,
                StudioID = media.StudioID,
                ActorIDs = media.Actor_SUB_Media.Select(n => n.ActorID).ToList(),
                GenreIDs = media.Genre_SUB_Media.Select(n => n.GenreID).ToList(),
                LanguageIDs = media.Language_SUB_Media.Select(n => n.LanguageID).ToList(),
                WriterIDs = media.Writer_SUB_Media.Select(n => n.WriterID).ToList(),
                ImageData = media.ImageData,
                ImageMimeType = media.ImageMimeType,
                YouTubeLink = media.YouTubeLink,
            };
        }

        // This method is for the index view. It retrieves a paginated list of all media and sends them to the view.
        [AllowAnonymous]
        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 9;

            var result = (await _service.GetAllAsync(n => n.Studio, m => m.Distributor, o => o.Producer))
                .ToPagedList(pageNumber, pageSize);

            return View(result);
        }

        // This method is for the filter view. It retrieves a filtered, paginated list of all media and sends them to the index view.
        [AllowAnonymous]
        public async Task<IActionResult> Filter(string searchString, int? page)
        {
            var result = await _service.GetAllAsync(n => n.Studio, m => m.Distributor, o => o.Producer);

            int pageSize = 9;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToLower();
                var resultFiltered = result.Where(n => n.Title.ToLower().Contains(searchString))
                                           .ToPagedList(pageNumber, pageSize);
                return View("Index", resultFiltered);
            }

            return View("Index", result.ToPagedList(pageNumber, pageSize));
        }

        // This method returns the details view for a specific media.
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var mediaDetail = await _service.GetMediaByIdAsync(id);
            return View(mediaDetail);
        }

        [Authorize(Roles = "admin, moderator")]
        // This method returns the create view for creating new media.
        public async Task<IActionResult> Create()
        {
            await PopulateDropdownValues();
            return View();
        }

        [Authorize(Roles = "admin, moderator")]
        // This method processes the POST request from the create view. It adds a new media.
        [HttpPost]
        public async Task<IActionResult> Create(MediaViewModel media, IFormFile ImageUpload)
        {
            await ProcessImageUploadAsync(ImageUpload, media);

            if (!ModelState.IsValid)
            {
                await PopulateDropdownValues();
                return View(media);
            }

            await _service.AddNewMediaAsync(media);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "admin, moderator")]
        // This method returns the edit view for a specific media.
        public async Task<IActionResult> Edit(int id)
        {
            var mediaDetails = await _service.GetMediaByIdAsync(id);

            if (mediaDetails == null)
            {
                return NotFound();
            }

            var response = MapToMediaViewModel(mediaDetails);

            await PopulateDropdownValues();

            return View(response);
        }

        [Authorize(Roles = "admin, moderator")]
        // This method processes the POST request from the edit view. It updates a specific media.
        [HttpPost]
        public async Task<IActionResult> Edit(int id, MediaViewModel media, IFormFile ImageUpload)
        {
            if (id != media.ID)
            {
                return NotFound();
            }

            await ProcessImageUploadAsync(ImageUpload, media);

            if (!ModelState.IsValid)
            {
                await PopulateDropdownValues();
                return View(media);
            }

            await _service.UpdateMediaAsync(media);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "admin, moderator")]
        [HttpPost, ActionName("DeleteConfirmed")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mediaDetails = await _service.GetMediaByIdAsync(id);
            if (mediaDetails == null)
            {
                return View("NotFound");
            }

            await _service.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }


        // This method adds a media to the user's "Plan To Watch" list. If the media is in the "Watched" list, it is removed from there first.
        public async Task<IActionResult> AddToPlanToWatch(int mediaId)
        {
            // Fetches the current user's ID.
            string userId = _userManager.GetUserId(User);

            // Checks if the media is in the user's "Watched" list.
            var watched = await _context.Watched.FirstOrDefaultAsync(w => w.MediaId == mediaId && w.UserId == userId);
            if (watched != null)
            {
                // If it is, the media is removed from the "Watched" list.
                await _service.RemoveFromWatchedAsync(mediaId, userId);
            }

            var planToWatch = new PlanToWatchModel
            {
                MediaId = mediaId,
                UserId = userId
            };

            // Adds the media to the user's "Plan To Watch" list.
            await _service.AddToPlanToWatchAsync(mediaId, userId);
            return RedirectToAction(nameof(Index));
        }

        // This method removes a media from the user's "Plan To Watch" list.
        public async Task<IActionResult> RemoveFromPlanToWatch(int mediaId)
        {
            // Fetches the current user's ID.
            string userId = _userManager.GetUserId(User);
            // Checks if the media is in the user's "Plan To Watch" list.
            var planToWatch = await _context.PlanToWatch.FirstOrDefaultAsync(ptw => ptw.MediaId == mediaId && ptw.UserId == userId);

            if (planToWatch != null)
            {
                // If it is, the media is removed from the "Plan To Watch" list.
                await _service.RemoveFromPlanToWatchAsync(mediaId, userId);
                return RedirectToAction(nameof(Index));
            }

            System.Diagnostics.Debug.WriteLine($"Media ID: {mediaId}");
            System.Diagnostics.Debug.WriteLine($"User ID: {userId}");
            System.Diagnostics.Debug.WriteLine($"PlanToWatch: {planToWatch}");

            // If it is not, an error message is displayed.
            TempData["FailureMessage"] = "Unable to remove media from Plan to Watch list.";
            return RedirectToAction(nameof(PlanToWatch));
        }

        [Authorize]
        // This method returns the "Plan To Watch" view for the user, displaying all media in their "Plan To Watch" list.
        public async Task<IActionResult> PlanToWatch(int? page)
        {
            // Fetches the current user's ID.
            var userId = _userManager.GetUserId(User);
            // Retrieves all media in the user's "Plan To Watch" list, along with their associated data.
            var planToWatchList = await _context.PlanToWatch
                .Include(ptw => ptw.Media)
                    .ThenInclude(media => media.Studio)
                .Include(ptw => ptw.Media)
                    .ThenInclude(media => media.Distributor)
                .Include(ptw => ptw.Media)
                    .ThenInclude(media => media.Producer)
                .Include(ptw => ptw.Media)
                    .ThenInclude(media => media.Genre_SUB_Media)
                        .ThenInclude(genre_sub_media => genre_sub_media.Genre)
                .Include(ptw => ptw.Media)
                    .ThenInclude(media => media.Actor_SUB_Media)
                        .ThenInclude(actor_sub_media => actor_sub_media.Actor)
                .Where(ptw => ptw.UserId == userId)
                .ToListAsync();

            int pageSize = 9;  
            int pageNumber = (page ?? 1);
            var pagedList = planToWatchList.ToPagedList(pageNumber, pageSize);

            // Calculates the total runtime of all media in the user's "Plan To Watch" list.
            var totalRuntime = planToWatchList.Sum(ptw => ptw.Media.Runtime);

            // Identifies the top 5 genres in the user's "Plan To Watch" list.
            var top5Genres = planToWatchList
                .SelectMany(ptw => ptw.Media.Genre_SUB_Media.Select(gsm => gsm.Genre))
                .GroupBy(genre => genre.Genre)
                .OrderByDescending(group => group.Count())
                .Take(5)
                .Select(group => new { Genre = group.Key, Count = group.Count() })
                .ToList();

            // Passes the total runtime and top 5 genres to the view.
            ViewBag.TotalRuntime = totalRuntime;
            ViewBag.Top5Genres = top5Genres;

            // Returns the paginated "Plan To Watch" list.
            return View(pagedList);
        }


        public async Task<IActionResult> AddToWatched(int mediaId)
        {
            string userId = _userManager.GetUserId(User);

            var planToWatch = await _context.PlanToWatch.FirstOrDefaultAsync(ptw => ptw.MediaId == mediaId && ptw.UserId == userId);
            if (planToWatch != null)
            {
                await _service.RemoveFromPlanToWatchAsync(mediaId, userId);
            }

            var watched = new WatchedModel
            {
                MediaId = mediaId,
                UserId = userId
            };

            await _service.AddToWatchedAsync(mediaId, userId);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RemoveFromWatched(int mediaId)
        {
            string userId = _userManager.GetUserId(User);
            var watched = await _context.Watched.FirstOrDefaultAsync(ptw => ptw.MediaId == mediaId && ptw.UserId == userId);

            if (watched != null)
            {
                await _service.RemoveFromWatchedAsync(mediaId, userId);
                return RedirectToAction(nameof(Index));
            }

            System.Diagnostics.Debug.WriteLine($"Media ID: {mediaId}");
            System.Diagnostics.Debug.WriteLine($"User ID: {userId}");
            System.Diagnostics.Debug.WriteLine($"watched: {watched}");

            TempData["FailureMessage"] = "Unable to remove media from Watched list.";
            return RedirectToAction(nameof(Watched));
        }

        [Authorize]
        public async Task<IActionResult> Watched(int? page)
        {
            var userId = _userManager.GetUserId(User);
            var watchedList = await _context.Watched
                .Include(ptw => ptw.Media)
                    .ThenInclude(media => media.Studio)
                .Include(ptw => ptw.Media)
                    .ThenInclude(media => media.Distributor)
                .Include(ptw => ptw.Media)
                    .ThenInclude(media => media.Producer)
                .Include(ptw => ptw.Media)
                    .ThenInclude(media => media.Genre_SUB_Media)
                        .ThenInclude(genre_sub_media => genre_sub_media.Genre)
                .Include(ptw => ptw.Media)
                    .ThenInclude(media => media.Actor_SUB_Media)
                        .ThenInclude(actor_sub_media => actor_sub_media.Actor)
                .Where(ptw => ptw.UserId == userId)
                .ToListAsync();

            int pageSize = 9;  
            int pageNumber = (page ?? 1);
            var pagedList = watchedList.ToPagedList(pageNumber, pageSize);

            var totalRuntime = watchedList.Sum(ptw => ptw.Media.Runtime);

            var top5Genres = watchedList
                .SelectMany(ptw => ptw.Media.Genre_SUB_Media.Select(gsm => gsm.Genre))
                .GroupBy(genre => genre.Genre)
                .OrderByDescending(group => group.Count())
                .Take(5)
                .Select(group => new { Genre = group.Key, Count = group.Count() })
                .ToList();
            var top5MediaTypes = watchedList
                .GroupBy(ptw => ptw.Media.Type)
                .OrderByDescending(group => group.Count())
                .Take(5)
                .Select(group => new { Type = group.Key, Count = group.Count() })
                .ToList();
            var top3Distributors = watchedList
                .GroupBy(ptw => ptw.Media.Distributor.Name)
                .OrderByDescending(group => group.Count())
                .Take(3)
                .Select(group => new { Distributor = group.Key, Count = group.Count() })
                .ToList();
            var top5Years = watchedList
                .Where(ptw => ptw.Media.Aired.HasValue)  
                .GroupBy(ptw => ptw.Media.Aired.Value.Year)
                .OrderByDescending(group => group.Count())
                .Take(5)
                .Select(group => new { Year = group.Key, Count = group.Count() })
                .ToList();

            var top5Actors = watchedList
                .Where(ptw => ptw.Media.Actor_SUB_Media != null) 
                .SelectMany(ptw => ptw.Media.Actor_SUB_Media.Select(asm => asm.Actor))
                .GroupBy(actor => actor.FullName)
                .OrderByDescending(group => group.Count())
                .Take(5)
                .Select(group => new { Actor = group.Key, Count = group.Count() })
                .ToList();

            var averageRuntime = watchedList.Any() ? watchedList.Average(ptw => ptw.Media.Runtime) : 0;


            ViewBag.TotalRuntime = totalRuntime;
            ViewBag.Top5Genres = top5Genres;
            ViewBag.Top5MediaTypes = top5MediaTypes;
            ViewBag.Top3Distributors = top3Distributors;
            ViewBag.Top5Years = top5Years;
            ViewBag.Top5Actors = top5Actors;
            ViewBag.AverageRuntime = averageRuntime;



            return View(pagedList);
        }

        [Authorize]
        // This method is used to generate media recommendations for the user based on their watched list. 
        public async Task<IActionResult> Recommendations(int? page)
        {
            // Fetches the current user's ID.
            var userId = _userManager.GetUserId(User);

            // Retrieves the IDs of all media in the user's "Watched" list.
            var watchedMediaIds = _context.Watched
                .Where(w => w.UserId == userId)
                .Select(w => w.MediaId).ToList();

            // Retrieves all media objects from the "Watched" list along with their associated data.
            var watchedMedia = _context.Media
                .Include(m => m.Genre_SUB_Media) 
                    .ThenInclude(gsm => gsm.Genre) 
                .Include(m => m.Studio) 
                .Include(m => m.Distributor)
                .Include(m => m.Producer) 
                .Where(m => watchedMediaIds.Contains(m.ID)).ToList();

            // Identifies the top 3 genres in the user's "Watched" list, and the number of media of each genre that the user has watched.
            var genreWeights = watchedMedia
                .SelectMany(m => m.Genre_SUB_Media.Select(gsm => gsm.Genre))
                .GroupBy(g => g)
                .OrderByDescending(g => g.Count()) 
                .Take(3) 
                .ToDictionary(g => g.Key, g => g.Count());

            // Retrieves all media not in the user's "Watched" list, along with their associated data.
            var otherMedia = _context.Media
                .Include(m => m.Genre_SUB_Media)
                    .ThenInclude(gsm => gsm.Genre)
                .Where(m => !watchedMediaIds.Contains(m.ID)) 
                .ToList();

            // For each media not in the "Watched" list, calculates a score based on the number of genres that match the user's top 3 genres.
            // A media's score is the sum of the watched count of each of its genres that are in the user's top 3 genres.
            // Only includes media that have a positive score, and sorts the media by score in descending order.
            var scoredMedia = otherMedia
                .Select(m => new
                {
                    Media = m,
                    Score = m.Genre_SUB_Media.Sum(gsm => genreWeights.GetValueOrDefault(gsm.Genre, 0))
                })
                .Where(m => m.Score > 0) 
                .OrderByDescending(m => m.Score) 
                .Select(m => m.Media);

            // Defines the size of each page for pagination.
            int pageSize = 10; 
            int pageNumber = (page ?? 1);

            // Returns the paginated list of recommended media.
            return View(scoredMedia.ToPagedList(pageNumber, pageSize));
        }



        private async Task<int> GetTotalRuntime()
        {
            var userId = _userManager.GetUserId(User);
            var watchedList = await _context.Watched
                .Include(ptw => ptw.Media)
                .Where(ptw => ptw.UserId == userId)
                .ToListAsync();

            return watchedList.Sum(ptw => ptw.Media.Runtime);
        }

        private async Task<List<string>> GetTop5Genres()
        {
            var userId = _userManager.GetUserId(User);
            var watchedList = await _context.Watched
                .Include(ptw => ptw.Media)
                    .ThenInclude(media => media.Genre_SUB_Media)
                        .ThenInclude(genre_sub_media => genre_sub_media.Genre)
                .Where(ptw => ptw.UserId == userId)
                .ToListAsync();

            var top5Genres = watchedList
                .SelectMany(ptw => ptw.Media.Genre_SUB_Media?.Select(gsm => gsm.Genre) ?? Enumerable.Empty<GenreModel>())
                .GroupBy(genre => genre.Genre)
                .OrderByDescending(group => group.Count())
                .Take(5)
                .Select(group => group.Key)
                .ToList();

            return top5Genres;
        }

        private async Task<List<string>> GetTop5Actors()
        {
            var userId = _userManager.GetUserId(User);
            var watchedList = await _context.Watched
                .Include(ptw => ptw.Media)
                    .ThenInclude(m => m.Actor_SUB_Media)
                        .ThenInclude(asm => asm.Actor)
                .Where(ptw => ptw.UserId == userId)
                .ToListAsync();

            return watchedList
                .Where(ptw => ptw.Media.Actor_SUB_Media != null)
                .SelectMany(ptw => ptw.Media.Actor_SUB_Media.Select(asm => asm.Actor))
                .GroupBy(actor => actor.FullName)
                .OrderByDescending(group => group.Count())
                .Take(5)
                .Select(group => group.Key)
                .ToList();
        }


        private async Task<List<string>> GetTop3RecommendedFilms()
        {
            var userId = _userManager.GetUserId(User);
            var watchedMediaIds = _context.Watched
                .Where(w => w.UserId == userId)
                .Select(w => w.MediaId).ToList();

            var watchedMedia = _context.Media
                .Include(m => m.Genre_SUB_Media)
                    .ThenInclude(gsm => gsm.Genre)
                .Where(m => watchedMediaIds.Contains(m.ID)).ToList();

            var genreWeights = watchedMedia
                .SelectMany(m => m.Genre_SUB_Media.Select(gsm => gsm.Genre))
                .GroupBy(g => g)
                .OrderByDescending(g => g.Count())
                .Take(3)
                .ToDictionary(g => g.Key, g => g.Count());

            var otherMedia = _context.Media
                .Include(m => m.Genre_SUB_Media)
                    .ThenInclude(gsm => gsm.Genre)
                .Where(m => !watchedMediaIds.Contains(m.ID))
                .ToList();

            return otherMedia
                .Select(m => new
                {
                    Media = m,
                    Score = m.Genre_SUB_Media.Sum(gsm => genreWeights.GetValueOrDefault(gsm.Genre, 0))
                })
                .Where(m => m.Score > 0)
                .OrderByDescending(m => m.Score)
                .Take(3)
                .Select(m => m.Media.Title)
                .ToList();
        }


        private async Task<int> GetTotalRuntimePlanToWatch()
        {
            var userId = _userManager.GetUserId(User);
            var planToWatchList = await _context.PlanToWatch
                .Include(ptw => ptw.Media)
                .Where(ptw => ptw.UserId == userId)
                .ToListAsync();

            return planToWatchList.Sum(ptw => ptw.Media.Runtime);
        }

        private async Task<List<string>> GetTop5GenresPlanToWatch()
        {
            var userId = _userManager.GetUserId(User);
            var planToWatchList = await _context.PlanToWatch
                .Include(ptw => ptw.Media)
                    .ThenInclude(media => media.Genre_SUB_Media)
                        .ThenInclude(genre_sub_media => genre_sub_media.Genre)
                .Where(ptw => ptw.UserId == userId)
                .ToListAsync();

            var top5Genres = planToWatchList
                .SelectMany(ptw => ptw.Media.Genre_SUB_Media?.Select(gsm => gsm.Genre) ?? Enumerable.Empty<GenreModel>())
                .GroupBy(genre => genre.Genre)
                .OrderByDescending(group => group.Count())
                .Take(5)
                .Select(group => group.Key)
                .ToList();

            return top5Genres;
        }

        private async Task<List<string>> GetTop5ActorsPlanToWatch()
        {
            var userId = _userManager.GetUserId(User);
            var planToWatchList = await _context.PlanToWatch
                .Include(ptw => ptw.Media)
                    .ThenInclude(m => m.Actor_SUB_Media)
                        .ThenInclude(asm => asm.Actor)
                .Where(ptw => ptw.UserId == userId)
                .ToListAsync();

            return planToWatchList
                .Where(ptw => ptw.Media.Actor_SUB_Media != null)
                .SelectMany(ptw => ptw.Media.Actor_SUB_Media.Select(asm => asm.Actor))
                .GroupBy(actor => actor.FullName)
                .OrderByDescending(group => group.Count())
                .Take(5)
                .Select(group => group.Key)
                .ToList();
        }

        // This method is used to generate a PDF report of the user's statistics and top 3 recommendations.
        public async Task<IActionResult> GenerateReport()
        {
            // Fetches various statistical and recommendation data by calling other methods within the class.
            var totalRuntime = await GetTotalRuntime();
            var top5Genres = await GetTop5Genres();
            var top5Actors = await GetTop5Actors();
            var top3Recommendations = await GetTop3RecommendedFilms();

            // Initializes an instance of StringBuilder to construct HTML content.
            StringBuilder htmlContent = new StringBuilder("<h1>Your statistics and top 3 recommendations</h1>");

            // Appends the total runtime to the HTML content.
            htmlContent.Append($"<p>Total runtime: {totalRuntime} minutes</p>");

            // Appends a list of top 5 genres to the HTML content.
            htmlContent.Append("<p>Top genres:</p>");
            htmlContent.Append("<ul>");
            foreach (var genre in top5Genres)
            {
                htmlContent.Append($"<li>{genre}</li>");
            }
            htmlContent.Append("</ul>");

            // Appends a list of top 5 actors to the HTML content.
            htmlContent.Append("<p>Top actors:</p>");
            htmlContent.Append("<ul>");
            foreach (var actor in top5Actors)
            {
                htmlContent.Append($"<li>{actor}</li>");
            }
            htmlContent.Append("</ul>");

            // Appends a list of top 3 recommended films to the HTML content.
            htmlContent.Append("<p>Top recommendations:</p>");
            htmlContent.Append("<ul>");
            foreach (var film in top3Recommendations)
            {
                htmlContent.Append($"<li>{film}</li>");
            }
            htmlContent.Append("</ul>");

            // Appends a thank you note to the HTML content.
            htmlContent.Append("Thank you for using our site!");

            // Converts the HTML content to a PDF document using IronPdf.
            var renderer = new IronPdf.HtmlToPdf();
            var pdf = renderer.RenderHtmlAsPdf(htmlContent.ToString());

            // Saves the generated PDF to the local file system.
            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "Report.pdf");
            pdf.SaveAs(outputPath);

            // Reads the PDF document from the local file system and prepares it for downloading.
            var fileBytes = System.IO.File.ReadAllBytes(outputPath);
            var response = new FileContentResult(fileBytes, "application/pdf")
            {
                FileDownloadName = "Report.pdf"
            };
            // Returns the PDF document as a file download to the user.
            return response;
        }

        public async Task<IActionResult> GenerateReportPlanToWatch()
        {
            var totalRuntime = await GetTotalRuntimePlanToWatch();
            var top5Genres = await GetTop5GenresPlanToWatch();
            var top5Actors = await GetTop5ActorsPlanToWatch();

            StringBuilder htmlContent = new StringBuilder("<h1>Your Plan to Watch statistics</h1>");

            htmlContent.Append($"<p>Total runtime: {totalRuntime} minutes</p>");

            htmlContent.Append("<p>Top genres:</p>");
            htmlContent.Append("<ul>");
            foreach (var genre in top5Genres)
            {
                htmlContent.Append($"<li>{genre}</li>");
            }
            htmlContent.Append("</ul>");

            htmlContent.Append("Thank you for using our site!");

            var renderer = new IronPdf.HtmlToPdf();
            var pdf = renderer.RenderHtmlAsPdf(htmlContent.ToString());

            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "PlanToWatchReport.pdf");
            pdf.SaveAs(outputPath);

            var fileBytes = System.IO.File.ReadAllBytes(outputPath);
            var response = new FileContentResult(fileBytes, "application/pdf")
            {
                FileDownloadName = "PlanToWatchReport.pdf"
            };
            return response;
        }



    }
}
