using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MovieScribe.Data.Base;
using MovieScribe.Data.Enumerators;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieScribe.Models
{
    public class MediaModel : IEntityBase
    {
        [Key]
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

        [Url(ErrorMessage = "Invalid URL")]
        public string? YouTubeLink { get; set; }

        public List<ActorsSUBMediaModel> Actor_SUB_Media { get; set; }
        public List<GenresSUBMediaModel> Genre_SUB_Media { get; set; }
        public List<LanguagesSUBMediaModel> Language_SUB_Media { get; set; }
        public List<WritersSUBMediaModel> Writer_SUB_Media { get; set; }
        public List<PlanToWatchModel> PlanToWatchMedia { get; set; }
        public List<WatchedModel> WatchedMedia { get; set; }

        public int ProducerID { get; set; }
        [ForeignKey("ProducerID")]
        public ProducerModel Producer { get; set; }

        public int DistributorID { get; set; }
        [ForeignKey("DistributorID")]
        public DistributorModel Distributor { get; set; }

        public int StudioID { get; set; }
        [ForeignKey("StudioID")]
        public StudioModel Studio { get; set; }
    }
}