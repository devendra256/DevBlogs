using System.ComponentModel.DataAnnotations;

namespace DevBlogs.Web.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [MinLength(6, ErrorMessage = "Password should have atleast 6 characters")]
        public string Password { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
