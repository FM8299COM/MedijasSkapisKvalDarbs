using Microsoft.EntityFrameworkCore;
using MovieScribe.Data.Base;
using MovieScribe.Data.ViewModels;
using MovieScribe.Models;

namespace MovieScribe.Data.Services
{
    // This class acts as the service layer for the Media entity in your application.
    // It contains methods that perform various operations on the Media entity and associated data.
    public class MediaService : EntityBaseRepo<MediaModel>, IMediaService
    {
        // An instance of your DbContext.
        private readonly DBContext _context;
        // The constructor of the class that initializes the DbContext.
        public MediaService(DBContext context) : base(context)
        {
            _context = context;
        }

        // Adds a new Media entry to the database.
        public async Task AddNewMediaAsync(MediaViewModel media)
        {
            // Translates the ViewModel to a Model.
            var newMedia = new MediaModel()
            {
                // Assigns properties from the ViewModel to the Model.
                Title = media.Title,
                Description= media.Description,
                Runtime = media.Runtime,
                Aired= media.Aired,
                Type = media.Type,
                ProducerID = media.ProducerID,
                StudioID = media.StudioID,
                DistributorID = media.DistributorID,
                ImageData = media.ImageData,
                ImageMimeType = media.ImageMimeType,
                YouTubeLink = media.YouTubeLink,

            };
            // Adds the new Media entry to the DbContext.
            await _context.Media.AddAsync(newMedia);
            // Saves the changes to the database.
            await _context.SaveChangesAsync();

            // Adds associated data for the new Media entry.
            // This includes actors, genres, languages, and writers.
            foreach (var actorID in media.ActorIDs)
            {
                var newActorMedia = new ActorsSUBMediaModel()
                {
                    MediaID = newMedia.ID,
                    ActorID = actorID,
                };
                await _context.Actors_SUB_Media.AddAsync(newActorMedia);
            }
            foreach (var genreID in media.GenreIDs)
            {
                var newGenreMedia = new GenresSUBMediaModel()
                {
                    MediaID = newMedia.ID,
                    GenreID = genreID,
                };
                await _context.Genres_SUB_Media.AddAsync(newGenreMedia);
            }
            foreach (var languageID in media.LanguageIDs)
            {
                var newLanguageMedia = new LanguagesSUBMediaModel()
                {
                    MediaID = newMedia.ID,
                    LanguageID = languageID,
                };
                await _context.Languages_SUB_Media.AddAsync(newLanguageMedia);
            }
            foreach (var writerID in media.WriterIDs)
            {
                var newWriterMedia = new WritersSUBMediaModel()
                {
                    MediaID = newMedia.ID,
                    WriterID = writerID,
                };
                await _context.Writers_SUB_Media.AddAsync(newWriterMedia);
            }
            await _context.SaveChangesAsync();
        }

        // Adds a Media entry to a user's "plan to watch" list.
        public async Task AddToPlanToWatchAsync(int mediaId, string userId)
        {
            // Searches for an existing entry in the "plan to watch" list.
            var planToWatch = await _context.PlanToWatch
                .FirstOrDefaultAsync(ptw => ptw.MediaId == mediaId && ptw.UserId == userId);

            // If no entry exists, creates a new one.
            if (planToWatch == null)
            {
                planToWatch = new PlanToWatchModel
                {
                    MediaId = mediaId,
                    UserId = userId
                };

                _context.PlanToWatch.Add(planToWatch);
                await _context.SaveChangesAsync();
            }
        }

        // Removes a Media entry from a user's "plan to watch" list.
        public async Task RemoveFromPlanToWatchAsync(int mediaId, string userId)
        {
            // Searches for an existing entry in the "plan to watch" list.
            var planToWatch = await _context.PlanToWatch
                .FirstOrDefaultAsync(ptw => ptw.MediaId == mediaId && ptw.UserId == userId);

            // If an entry exists, removes it.
            if (planToWatch != null)
            {
                _context.PlanToWatch.Remove(planToWatch);
                await _context.SaveChangesAsync();
            }
        }

        // Adds a Media entry to a user's "watched" list.
        public async Task AddToWatchedAsync(int mediaId, string userId)
        {
            // Similar to AddToPlanToWatchAsync, but for the "watched" list.
            var watched = await _context.Watched
                .FirstOrDefaultAsync(ptw => ptw.MediaId == mediaId && ptw.UserId == userId);

            if (watched == null)
            {
                watched = new WatchedModel
                {
                    MediaId = mediaId,
                    UserId = userId
                };

                _context.Watched.Add(watched);
                await _context.SaveChangesAsync();
            }
        }

        // Removes a Media entry from a user's "watched" list.
        public async Task RemoveFromWatchedAsync(int mediaId, string userId)
        {
            // Similar to RemoveFromPlanToWatchAsync, but for the "watched" list.
            var watched = await _context.Watched
                .FirstOrDefaultAsync(ptw => ptw.MediaId == mediaId && ptw.UserId == userId);

            if (watched != null)
            {
                _context.Watched.Remove(watched);
                await _context.SaveChangesAsync();
            }
        }

        // Fetches a Media entry by ID, including related data.
        public async Task<MediaModel> GetMediaByIdAsync(int id)
        {
            // Performs a query that includes related data.
            var mediadetails = await _context.Media
                .Include(s => s.Studio)
                .Include(d => d.Distributor)
                .Include(d => d.Distributor)
                .Include(p => p.Producer)
                .Include(am => am.Actor_SUB_Media).ThenInclude(a => a.Actor)
                .Include(gm => gm.Genre_SUB_Media).ThenInclude(g => g.Genre)
                .Include(lm => lm.Language_SUB_Media).ThenInclude(l => l.Language)
                .Include(wm => wm.Writer_SUB_Media).ThenInclude(w => w.Writer)
                .FirstOrDefaultAsync(n => n.ID == id);

            mediadetails.ImageData = mediadetails.ImageData;
            mediadetails.ImageMimeType = mediadetails.ImageMimeType;
            mediadetails.YouTubeLink = mediadetails.YouTubeLink;

            // Returns the result of the query.
            return mediadetails;
        }

        // Fetches values for a dropdown list in a view.
        public async Task<MediaDropdownViewModel> GetMediaDropdownViewValues()
        {
            // Creates a new ViewModel and assigns values to it.
            // The values are lists of entities ordered by name or ID.
            var response = new MediaDropdownViewModel()
            {
                Producers = await _context.Producers.OrderBy(a => a.Name).ToListAsync(),
                Distributors = await _context.Distributors.OrderBy(a => a.Name).ToListAsync(),
                Studios = await _context.Studios.OrderBy(a => a.Name).ToListAsync(),
                Actors = await _context.Actors.OrderBy(a => a.Name).ToListAsync(),
                Genres = await _context.Genres.OrderBy(a => a.ID).ToListAsync(),
                Languages = await _context.Languages.OrderBy(a => a.ID).ToListAsync(),
                Writers = await _context.Writers.OrderBy(a => a.Name).ToListAsync()
            };

            return response;
        }

        // Updates an existing Media entry.
        public async Task UpdateMediaAsync(MediaViewModel media)
        {
            // Searches for an existing Media entry.
            var dbMedia = await _context.Media.FirstOrDefaultAsync(n => n.ID == media.ID);

            // If an entry exists, updates its properties.
            if (dbMedia != null)
            {
                dbMedia.Title = media.Title;
                dbMedia.Description = media.Description;
                dbMedia.Runtime = media.Runtime;
                dbMedia.Aired = media.Aired;
                dbMedia.Type = media.Type;
                dbMedia.ProducerID = media.ProducerID;
                dbMedia.StudioID = media.StudioID;
                dbMedia.DistributorID = media.DistributorID;
                dbMedia.YouTubeLink = media.YouTubeLink;

                // If the ViewModel includes image data, updates the image data in the Model.
                if (media.ImageData != null && media.ImageMimeType != null)
                {
                    dbMedia.ImageData = media.ImageData;
                    dbMedia.ImageMimeType = media.ImageMimeType;
                }

                await _context.SaveChangesAsync();
            }

            // Removes and re-adds associated data for the Media entry.
            // This includes actors, genres, languages, and writers.
            var existingActors = _context.Actors_SUB_Media.Where(n => n.MediaID == media.ID).ToList();
             _context.Actors_SUB_Media.RemoveRange(existingActors);
            await _context.SaveChangesAsync();



            foreach (var actorID in media.ActorIDs)
            {
                var existingActor = await _context.Actors_SUB_Media
        .Where(n => n.MediaID == media.ID && n.ActorID == actorID)
        .FirstOrDefaultAsync();

                if (existingActor == null)
                {
                    var newActorMedia = new ActorsSUBMediaModel()
                    {
                        MediaID = media.ID,
                        ActorID = actorID,
                    };
                    await _context.Actors_SUB_Media.AddAsync(newActorMedia);
                }
            }
            await _context.SaveChangesAsync();
            foreach (var genreID in media.GenreIDs)
            {
                var existingGenre = await _context.Genres_SUB_Media
        .Where(n => n.MediaID == media.ID && n.GenreID == genreID)
        .FirstOrDefaultAsync();

                if (existingGenre == null)
                {
                    var newGenreMedia = new GenresSUBMediaModel()
                    {
                        MediaID = media.ID,
                        GenreID = genreID,
                    };
                    await _context.Genres_SUB_Media.AddAsync(newGenreMedia);
                }
            }
            await _context.SaveChangesAsync();
            foreach (var languageID in media.LanguageIDs)
            {
                var existingLanguage = await _context.Languages_SUB_Media
        .Where(n => n.MediaID == media.ID && n.LanguageID == languageID)
        .FirstOrDefaultAsync();

                if (existingLanguage == null)
                {
                    var newLanguageMedia = new LanguagesSUBMediaModel()
                    {
                        MediaID = media.ID,
                        LanguageID = languageID,
                    };
                    await _context.Languages_SUB_Media.AddAsync(newLanguageMedia);
                }
            }
            await _context.SaveChangesAsync();
            foreach (var writerID in media.WriterIDs)
            {
                var existingWriter = await _context.Writers_SUB_Media
        .Where(n => n.MediaID == media.ID && n.WriterID == writerID)
        .FirstOrDefaultAsync();

                if (existingWriter == null)
                {
                    var newWriterMedia = new WritersSUBMediaModel()
                    {
                        MediaID = media.ID,
                        WriterID = writerID,
                    };
                    await _context.Writers_SUB_Media.AddAsync(newWriterMedia);
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
