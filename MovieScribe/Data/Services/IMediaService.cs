using MovieScribe.Data.Base;
using MovieScribe.Data.ViewModels;
using MovieScribe.Models;

namespace MovieScribe.Data.Services
{
    public interface IMediaService : IEntityBaseRepo<MediaModel>
    {
        Task<MediaModel> GetMediaByIdAsync(int id);

        Task<MediaDropdownViewModel> GetMediaDropdownViewValues();
        Task AddNewMediaAsync(MediaViewModel media);
        Task UpdateMediaAsync(MediaViewModel media);

        Task AddToPlanToWatchAsync(int mediaId, string userId);
        Task RemoveFromPlanToWatchAsync(int mediaId, string userId);

        Task AddToWatchedAsync(int mediaId, string userId);
        Task RemoveFromWatchedAsync(int mediaId, string userId);

    }
}
