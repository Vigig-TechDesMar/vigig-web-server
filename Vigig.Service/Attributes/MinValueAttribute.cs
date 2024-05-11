using System.ComponentModel.DataAnnotations;

namespace Vigig.Service.Attributes;
[AttributeUsage(AttributeTargets.Property)]
public class MinValueAttribute : ValidationAttribute
{
    protected readonly double _minValue;

    public MinValueAttribute(double minValue)
    {
        _minValue = minValue;
    }

    protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
    {
        if (value == null || !(value is IComparable))
        {
            return ValidationResult.Success;
        }

        var comparableValue = (IComparable)value;
        if (comparableValue.CompareTo(_minValue) < 0)
        {
            return new ValidationResult(
                $"The field {validationContext.DisplayName} must be greater than or equal to {_minValue}.");
        }

        return ValidationResult.Success;
    }
}