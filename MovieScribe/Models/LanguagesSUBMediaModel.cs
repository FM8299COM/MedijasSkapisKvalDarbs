using System.ComponentModel.DataAnnotations;

namespace MovieScribe.Models
{
    public class LanguagesSUBMediaModel
    {
        [Required]
        public int MediaID { get; set; }
        public MediaModel Media { get; set; }

        [Required]
        public int LanguageID { get; set; }
        public LanguageModel Language { get; set; }
    }
}