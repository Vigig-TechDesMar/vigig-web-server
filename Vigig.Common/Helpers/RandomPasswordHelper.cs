namespace Vigig.Common.Helpers;

public static class RandomPasswordHelper
{
    private static readonly Random Random = new Random();
    public static string GenerateRandomPassword(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        const string specialChars = "!@#$%^&*()_+";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Next(s.Length)])
            .ToArray()
            .Append(specialChars[Random.Next(specialChars.Length)]).ToArray());
    }
    public static string GenerateRandomDigitPassword(int length)
    {
        const string digits = "0123456789";
        return new string(Enumerable.Repeat(digits, length)
            .Select(s => s[Random.Next(s.Length)])
            .ToArray());
    }
}