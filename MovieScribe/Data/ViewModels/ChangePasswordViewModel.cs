using System.ComponentModel.DataAnnotations;

namespace MovieScribe.Data.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Pašreizējā parole ir nepieciešams lauks")]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Jaunā parole ir nepieciešams lauks")]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Apstiprini jauno paroli")]
        [Compare("NewPassword", ErrorMessage = "Jaunā un vecā parole sakrīt")]
        public string ConfirmPassword { get; set; }
    }
}
