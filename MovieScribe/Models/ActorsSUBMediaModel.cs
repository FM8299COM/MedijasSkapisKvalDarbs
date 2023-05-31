using System.ComponentModel.DataAnnotations;

namespace MovieScribe.Models
{
    public class ActorsSUBMediaModel
    {
        [Required]
        public int MediaID { get; set; }
        public MediaModel Media { get; set; }

        [Required]
        public int ActorID { get; set; }
        public ActorModel Actor { get; set; }
    }
}