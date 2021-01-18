using System.ComponentModel.DataAnnotations;

namespace FingergunsApi.App.Data.Contracts.Requests
{
    public class LoginRequest
    {
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; } = false;
    }
}