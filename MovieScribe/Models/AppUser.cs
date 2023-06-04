using Microsoft.AspNetCore.Identity;

namespace MovieScribe.Models
{
    public class AppUser : IdentityUser
    {
        public byte[]? ProfilePictureData { get; set; }
        public string? ProfilePictureMimeType { get; set; }

        public ICollection<PlanToWatchModel> PlanToWatchMedia { get; set; }
        public ICollection<WatchedModel> WatchedMedia { get; set; }
    }
}
