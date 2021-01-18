using System.ComponentModel.DataAnnotations;

namespace FingergunsApi.App.Data.Validations
{
    public class EqualsAttribute : ValidationAttribute
    {
        private string Value { get; }

        public EqualsAttribute(string value)
        {
            Value = value;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var field = (string) value;
            if (!field.Equals(Value))
            {
                var fieldName = validationContext.DisplayName;
                return new ValidationResult($"Field ${fieldName} must be equal to ${value}");
            }
            
            return ValidationResult.Success;
        }
    }
}