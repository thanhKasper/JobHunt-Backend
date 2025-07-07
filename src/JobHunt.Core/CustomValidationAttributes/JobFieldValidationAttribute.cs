using System.ComponentModel.DataAnnotations;
using JobHunt.Core.Domain.ValueObjects;

namespace JobHunt.Core.CustomValidationAttributes;

public class JobFieldValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value != null)
        {
            string checkedJobLevel = (string)value;
            if (String.IsNullOrEmpty(checkedJobLevel)) return ValidationResult.Success;
            if (!Enum.TryParse((string)value, true, out JobFieldKey jobField))
            {
                return new ValidationResult(ErrorMessage ?? $"Fail to convert \"{(string)value}\" to valid value");
            }

            return ValidationResult.Success;
        }

        return null;
    }
}