namespace Vigig.Common.Extensions;

public static class StringExtension
{
    public static bool ToBool(this string value)
    {
        return value.Trim().ToLower() == "true";
    }
}