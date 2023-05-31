using System.ComponentModel.DataAnnotations;

namespace MovieScribe.Models
{
    public class GenreModel
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Genre name is a required field")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Genre name cannot be less than 1 character or more than 255")]
        public string Genre { get; set; }

        public List<GenresSUBMediaModel>? Genre_SUB_Media { get; set; }

    }
}