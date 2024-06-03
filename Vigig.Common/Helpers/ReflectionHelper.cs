namespace Vigig.Common.Helpers;

public class ReflectionHelper
{
    public static string GetStringValueByName(Type type, string propertyName, object obj)
    {
        var property = type.GetProperties()
            .FirstOrDefault(x => string.Equals(x.Name, propertyName, StringComparison.OrdinalIgnoreCase));
        var propertyValue = property.GetValue(obj);
        return propertyValue?.ToString() ?? string.Empty;
    }

    public static object GetValueByName(Type type, string propertyName, object obj)
    {
        var property = type.GetProperties()
            .FirstOrDefault(x => string.Equals(x.Name, propertyName, StringComparison.OrdinalIgnoreCase));
        var propertyValue = property.GetValue(obj);
        return propertyValue ?? 0;
    }


}