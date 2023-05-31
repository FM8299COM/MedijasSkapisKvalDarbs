using System.ComponentModel.DataAnnotations;

namespace MovieScribe.Models
{
    public class GenresSUBMediaModel
    {
        [Required]
        public int MediaID { get; set; }
        public MediaModel Media { get; set; }

        [Required]
        public int GenreID { get; set; }
        public GenreModel Genre { get; set; }
    }
}