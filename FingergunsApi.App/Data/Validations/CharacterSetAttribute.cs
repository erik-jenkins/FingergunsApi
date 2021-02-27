using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FingergunsApi.App.Data.Validations
{
    public class SpecialCharactersAttribute : ValidationAttribute
    {
        private const string SpecialCharacterSet = @"!""#$%&\'()*+,-./:;<=>?@[\\]^_`{|}~";

        private int MinNumber { get; }

        public SpecialCharactersAttribute(int minNumber)
        {
            MinNumber = minNumber;
        }

        private string GetErrorMessage(string displayName) =>
            $"{displayName} must contain at least {MinNumber} special character{(MinNumber == 1 ? "" : "s")}.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var field = (string) value;
            var numChars = field
                .Where(c => SpecialCharacterSet.Contains(c))
                .Select(_ => 1)
                .Sum();

            return numChars < MinNumber ? new ValidationResult(GetErrorMessage(validationContext.DisplayName)) : ValidationResult.Success;
        }
    }

    public class NumberCharactersAttribute : ValidationAttribute
    {
        private const string NumberCharacterSet = "1234567890";
        
        private int MinNumber { get; }

        public NumberCharactersAttribute(int minNumber)
        {
            MinNumber = minNumber;
        }

        private string GetErrorMessage(string displayName) =>
            $"{displayName} must contain at least {MinNumber} numeric character{(MinNumber == 1 ? "" : "s")}.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var field = (string) value;
            var numChars = field
                .Where(c => NumberCharacterSet.Contains(c))
                .Select(_ => 1)
                .Sum();

            return numChars < MinNumber ? new ValidationResult(GetErrorMessage(validationContext.DisplayName)) : ValidationResult.Success;
        }
    }
}