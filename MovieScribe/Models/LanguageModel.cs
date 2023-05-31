using System.ComponentModel.DataAnnotations;

namespace MovieScribe.Models
{
    public class LanguageModel
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Language name is a required field")]
        [StringLength(255, MinimumLength = 1, ErrorMessage = "Language name cannot be less than 1 character or more than 255")]
        public string Language { get; set; }

        public List<LanguagesSUBMediaModel>? Language_SUB_Media { get; set; }

    }
}