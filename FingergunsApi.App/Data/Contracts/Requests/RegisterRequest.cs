using System.ComponentModel.DataAnnotations;
using FingergunsApi.App.Data.Validations;

namespace FingergunsApi.App.Data.Contracts.Requests
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Display name is required.")]
        [MaxLength(16, ErrorMessage = "Display names must be 16 or fewer characters.")]
        public string DisplayName { get; set; }
        
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        [SpecialCharacters(1)]
        [NumberCharacters(1)]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Password confirmation is required.")]
        public string Confirm { get; set; }
    }
}