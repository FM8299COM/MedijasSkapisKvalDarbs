using System.ComponentModel.DataAnnotations;

namespace MovieScribe.Data.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Lietotājvārds ir nepieciešam lauks")]
        public string UserName { get; set; }

        [Display(Name = "Profile Picture")]
        public IFormFile? ProfilePicture { get; set; }

        [Display(Name = "Email address")]
        [Required(ErrorMessage = "E-pasta adrese ir nepieciešams lauks")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Parole ir nepieciešams lauks")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Paroles salīdzināšanas ir nepieciešams lauks")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Paroles nesakrīt")]
        public string ConfirmPassword { get; set; }
    }
}
