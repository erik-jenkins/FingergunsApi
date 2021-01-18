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

        public string GetErrorMessage() =>
            $"Field must contain at least {MinNumber} characters of the specified character set.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var field = (string) value;
            var numChars = field
                .Where(c => SpecialCharacterSet.Contains(c))
                .Select(_ => 1)
                .Sum();

            return numChars < MinNumber ? new ValidationResult(GetErrorMessage()) : ValidationResult.Success;
        }
    }

    public class NumberCharactersAttribute : ValidationAttribute
    {
        public const string NumberCharacterSet = "1234567890";
        private int MinNumber { get; }

        public NumberCharactersAttribute(int minNumber)
        {
            MinNumber = minNumber;
        }

        public string GetErrorMessage() =>
            $"Field must contain at least {MinNumber} characters of the specified character set.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var field = (string) value;
            var numChars = field
                .Where(c => NumberCharacterSet.Contains(c))
                .Select(_ => 1)
                .Sum();

            return numChars < MinNumber ? new ValidationResult(GetErrorMessage()) : ValidationResult.Success;
        }
    }
}