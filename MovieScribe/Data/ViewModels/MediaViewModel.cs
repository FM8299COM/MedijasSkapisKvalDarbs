using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MovieScribe.Data.Base;
using MovieScribe.Data.Enumerators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieScribe.Models
{
    public class MediaViewModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Title is a required field")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Title cannot be less than 1 character or more than 255")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Runtime is a required field")]
        [Range(1, 999, ErrorMessage = "Media cannot be shorter than 1 minute or longer than 999 minutes")]
        public int Runtime { get; set; }

        [MinLength(1)]
        public string? Description { get; set; }

        [NotMapped]
        public DateOnly? Aired
        {
            get => AiredDateTime.HasValue ? DateOnly.FromDateTime(AiredDateTime.Value) : null;
            set => AiredDateTime = value?.ToDateTime(TimeOnly.MinValue);
        }

        [Column("Aired")]
        [Range(typeof(DateTime), "1800-01-01","9999-12-31", ErrorMessage = "Media cannot have come out before the 1800's and after the year 9999")]
        public DateTime? AiredDateTime { get; set; }
        public Data.Enumerators.MediaType Type { get; set; }

        public byte[]? ImageData { get; set; }
        public string? ImageMimeType { get; set; }
        public IFormFile? ImageUpload { get; set; }

        [Url(ErrorMessage = "Invalid URL")]
        public string? YouTubeLink { get; set; }

        [Display(Name = "Select actor/s")]
        [Required(ErrorMessage = "This is required field")]
        public List<int> ActorIDs { get; set; }
        [Display(Name = "Select genre/s")]
        [Required(ErrorMessage = "This is required field")]
        public List<int> GenreIDs { get; set; }
        [Display(Name = "Select language/s")]
        [Required(ErrorMessage = "This is required field")]
        public List<int> LanguageIDs { get; set; }
        [Display(Name = "Select writer/s")]
        [Required(ErrorMessage = "This is required field")]
        public List<int> WriterIDs { get; set; }
        [Display(Name = "Select a producer")]
        [Required(ErrorMessage = "This is required field")]
        public int ProducerID { get; set; }
        [Display(Name = "Select a distributor")]
        [Required(ErrorMessage = "This is required field")]
        public int DistributorID { get; set; }
        [Display(Name = "Select a studio")]
        [Required(ErrorMessage = "This is required field")]
        public int StudioID { get; set; }
    }
}