using System.ComponentModel.DataAnnotations;

namespace MovieScribe.Models
{
    public class WritersSUBMediaModel
    {
        [Required]
        public int MediaID { get; set; }
        public MediaModel Media { get; set; }

        [Required]
        public int WriterID { get; set; }
        public WriterModel Writer { get; set; }
    }
}