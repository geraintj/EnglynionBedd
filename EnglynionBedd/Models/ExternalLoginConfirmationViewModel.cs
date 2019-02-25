﻿using System.ComponentModel.DataAnnotations;

namespace EnglynionBedd.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}