using System.ComponentModel.DataAnnotations;

namespace BlogWeb.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
