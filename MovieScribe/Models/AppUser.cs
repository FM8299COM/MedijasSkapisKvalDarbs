using Microsoft.AspNetCore.Identity;

namespace MovieScribe.Models
{
    public class AppUser : IdentityUser
    {
        public ICollection<PlanToWatchModel> PlanToWatchMedia { get; set; }
        public ICollection<WatchedModel> WatchedMedia { get; set; }
    }
}
