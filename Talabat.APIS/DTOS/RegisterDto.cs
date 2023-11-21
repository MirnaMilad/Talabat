﻿using System.ComponentModel.DataAnnotations;

namespace Talabat.APIS.DTOS
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
