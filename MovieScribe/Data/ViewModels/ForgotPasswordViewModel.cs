﻿using System.ComponentModel.DataAnnotations;

namespace MovieScribe.Data.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
