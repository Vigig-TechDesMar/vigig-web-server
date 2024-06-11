using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Vigig.Common.Helpers;
public class EnumHelper
{
    private static readonly Dictionary<Type, Dictionary<string, string>> EnumTranslations =
        new Dictionary<Type, Dictionary<string, string>>();

    public static T GetEnumValueFromString<T>(string valueName) where T : struct, Enum
    {
        if (Enum.TryParse<T>(valueName, out T result))
        {
            return result;
        }
        else
        {
            throw new ArgumentException($"Enum does not contain value {valueName}");
        }
    }
    public static List<T> GetEnumValuesFromStrings<T>(IEnumerable<string> valueNames) where T : struct, Enum
    {
        List<T> enumValues = new List<T>();

        foreach (var valueName in valueNames)
        {
            if(string.IsNullOrWhiteSpace(valueName))
                continue;
            var capitalizedValue = valueName.ToCapitalized();

            if (Enum.TryParse<T>(capitalizedValue, out T result))
            {
                enumValues.Add(result);
            }
            else
            {
                throw new ArgumentException($"Enum does not contain value {capitalizedValue}");
            }
        }

        return enumValues;
    }
    public static string TranslateEnum(Enum enumValue)
    {
        Type enumType = enumValue.GetType();

        if (!EnumTranslations.ContainsKey(enumType))
        {
            InitializeEnumTranslations(enumType);
        }

        Dictionary<string, string> translations = EnumTranslations[enumType];
        string enumName = enumValue.ToString();

        if (translations.ContainsKey(enumName))
        {
            return translations[enumName];
        }

        return enumName;
    }

    private static void InitializeEnumTranslations(Type enumType)
    {
        var translations = new Dictionary<string, string>();

        foreach (FieldInfo field in enumType.GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            DescriptionAttribute? descriptionAttribute = field.GetCustomAttribute<DescriptionAttribute>();
            string enumName = field.Name;
            string description = descriptionAttribute?.Description ?? enumName;

            translations.Add(enumName, description);
        }

        EnumTranslations[enumType] = translations;
    }
}
