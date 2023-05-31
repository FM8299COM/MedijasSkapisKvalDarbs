using MovieScribe.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace MovieScribe.Models
{
    public class ActorModel : IEntityBase
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Name is a required field")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Name cannot be less than 1 character or more than 255")]
        public string Name { get; set; }

        [MinLength(1)]
        public string? Middle_Name { get; set; }
        
        [Required(ErrorMessage = "Surname is a required field")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Surname cannot be less than 1 character or more than 255")]
        public string Surname { get; set; }

        public string FullName { get { return Name + " " + Middle_Name + " " + Surname; } }

        public byte[]? ImageData { get; set; }
        public string? ImageMimeType { get; set; }

        [Required(ErrorMessage = "Age is a required field")]
        [Range(1, 150, ErrorMessage = "Age cannot be less 1 or more than 150")]
        public int Age { get; set; }

        [MinLength(1)]
        public string? Biography { get; set; }

        public List<ActorsSUBMediaModel>? Actor_SUB_Media { get; set; }
    }
}