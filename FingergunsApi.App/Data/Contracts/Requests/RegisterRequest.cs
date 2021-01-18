using FingergunsApi.App.Data.Validations;

namespace FingergunsApi.App.Dtos.Requests
{
    public class RegisterRequest : ValidationRoot
    {
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        public string Confirm { get; set; }
        
        public override bool IsValid()
        {
            if (!IsPresent(Email) || !IsValidEmail(Email))
                return false;

            if (!IsPresent(DisplayName))
                return false;

            if (!IsPresent(Password) || 
                !IsLengthAtLeast(Password, 8) ||
                !ContainsAtLeastNCharacters(Password, 1, SpecialCharacterSet) ||
                !ContainsAtLeastNCharacters(Password, 1, NumberCharacterSet))
                return false;

            if (Confirm != Password)
                return false;
            
            return true;
        }
    }
}