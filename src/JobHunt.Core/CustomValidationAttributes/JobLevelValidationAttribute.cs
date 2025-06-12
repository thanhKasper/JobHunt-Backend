using System.ComponentModel.DataAnnotations;
using JobHunt.Core.Domain.ValueObjects;

namespace JobHunt.Core.CustomValidationAttributes;
public class JobLevelValidationAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value != null)
        {
            string checkedJobLevel = (string)value;
            if (!Enum.TryParse((string)value, true, out JobLevel jobLevel))
            {
                return new ValidationResult(ErrorMessage ?? $"Fail to convert \"{(string)value}\" to valid value");
            }
            else
            {
                return ValidationResult.Success;
            }
        }

        return ValidationResult.Success;

    }
}