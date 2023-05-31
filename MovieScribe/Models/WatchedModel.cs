using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieScribe.Models
{
    public class WatchedModel
    {
        public string UserId { get; set; }
        public AppUser User { get; set; }

        public int MediaId { get; set; }
        public MediaModel Media { get; set; }
    }
}
