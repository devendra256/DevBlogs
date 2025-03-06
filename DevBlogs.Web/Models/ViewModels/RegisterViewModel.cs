﻿using System.ComponentModel.DataAnnotations;

namespace DevBlogs.Web.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [MinLength(6, ErrorMessage = "Password should have atleast 6 characters")]
        public string Password { get; set; }
    }
}
