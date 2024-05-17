using System.ComponentModel.DataAnnotations;

namespace Vigig.Service.Attributes;

public class NotEmptyOrWhiteSpaceAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        var isValid = !string.IsNullOrWhiteSpace(value.ToString());
        if (!isValid)
            return new ValidationResult("The property should not be null or empty or contains whitespace only.");
        return ValidationResult.Success;
    }   
}