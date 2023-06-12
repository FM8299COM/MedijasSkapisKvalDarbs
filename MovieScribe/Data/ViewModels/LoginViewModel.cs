using System.ComponentModel.DataAnnotations;

namespace MovieScribe.Data.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Email address")]
        [Required(ErrorMessage = "E-pasta adrese ir nepieciešams lauks")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Parole ir nepieciešams lauks")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
