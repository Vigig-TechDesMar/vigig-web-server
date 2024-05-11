using System.ComponentModel.DataAnnotations;

namespace Vigig.Service.Attributes;

public class GreaterThanOrEqualAttribute: ValidationAttribute
{
    private readonly string _otherPropertyName;

    public GreaterThanOrEqualAttribute(string otherPropertyName)
    {
        _otherPropertyName = otherPropertyName;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var otherPropertyInfo = validationContext.ObjectType.GetProperty(_otherPropertyName);
        if (otherPropertyInfo is null)
            return new ValidationResult($"Unknown Property: {_otherPropertyName}");
        var otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance);
        if (value == null || otherPropertyValue == null || !(value is IComparable) || !(otherPropertyValue is IComparable))
            return ValidationResult.Success;
        var thisValue = (IComparable)value;
        var otherValue = (IComparable)otherPropertyValue;
        if (thisValue.CompareTo(otherValue) <= 0)
            return new ValidationResult(
                $"The field {validationContext.DisplayName} must be greather than {_otherPropertyName}.");
        return ValidationResult.Success;
    }
    
}