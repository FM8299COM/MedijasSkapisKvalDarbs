using MovieScribe.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace MovieScribe.Models
{
    public class StudioModel : IEntityBase
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Studio name is a required field")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Studio name cannot be less than 1 character or more than 255")]
        public string Name { get; set; }

        [MinLength(1)]
        public string? Description { get; set; }

        public byte[]? ImageData { get; set; }
        public string? ImageMimeType { get; set; }

        public List<MediaModel>? Media { get; set; }

    }
}