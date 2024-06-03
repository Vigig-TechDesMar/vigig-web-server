using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

public class LaterThanOrAtTheSameTimeAttribute : ValidationAttribute
{
    private readonly string _otherPropertyName;

    public LaterThanOrAtTheSameTimeAttribute(string otherPropertyName)
    {
        _otherPropertyName = otherPropertyName;
    }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        var otherPropertyInfo = validationContext.ObjectType.GetProperty(_otherPropertyName);
        if (otherPropertyInfo is null)
            return new ValidationResult($"Unknown Property: {_otherPropertyName}");

        var otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance);
        
        // Handle null cases if properties are nullable
        if (value == null || otherPropertyValue == null)
            return ValidationResult.Success!;

        if (value is IComparable thisValue && otherPropertyValue is IComparable otherValue)
        {
            if (thisValue.CompareTo(otherValue) < 0)
                return new ValidationResult(
                    $"The field {validationContext.DisplayName} must be later than or at the same time as {_otherPropertyName}.");
        }
        else
            return new ValidationResult("The compared fields must be of comparable types.");

        return ValidationResult.Success!;
    }
}