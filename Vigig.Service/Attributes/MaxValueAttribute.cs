using System.ComponentModel.DataAnnotations;

namespace Vigig.Service.Attributes;
[AttributeUsage(AttributeTargets.Property)]
public class MaxValueAttribute : ValidationAttribute
{
    protected readonly double _maxValue;

    public MaxValueAttribute(double maxValue)
    {
        _maxValue = maxValue;
    }

    protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
    {
        if (value == null || !(value is IComparable))
        {
            return ValidationResult.Success;
        }

        var comparableValue = (IComparable)value;
        if (comparableValue.CompareTo(_maxValue) > 0)
        {
            return new ValidationResult(
                $"The field {validationContext.DisplayName} must be smaller than or equal to {_maxValue}.");
        }

        return ValidationResult.Success;
    }
}