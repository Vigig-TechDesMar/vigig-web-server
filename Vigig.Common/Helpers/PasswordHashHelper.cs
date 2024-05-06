using System.Security.Cryptography;

namespace Vigig.Common.Helpers;

public static class PasswordHashHelper
{
    private const int SaltSize = 16; // 16 bytes = 128 bits
    private const int HashSize = 20; // 20 bytes = 160 bits
    private const int Iterations = 10000;

    public static string HashPassword(string password)
    {
        // Generate a random salt
        byte[] salt;
        new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

        // Hash the password with the salt using PBKDF2
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
        byte[] hash = pbkdf2.GetBytes(HashSize);

        // Combine the salt and hash into a single string
        byte[] hashBytes = new byte[SaltSize + HashSize];
        Array.Copy(salt, 0, hashBytes, 0, SaltSize);
        Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

        // Convert to base64-encoded string for storage
        string hashedPassword = Convert.ToBase64String(hashBytes);

        return hashedPassword;
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        // Extract the salt and hash from the stored password
        byte[] hashBytes = Convert.FromBase64String(hashedPassword);
        byte[] salt = new byte[SaltSize];
        Array.Copy(hashBytes, 0, salt, 0, SaltSize);

        // Compute the hash of the password using the salt
        var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations);
        byte[] hash = pbkdf2.GetBytes(HashSize);

        // Compare the computed hash with the stored hash
        for (int i = 0; i < HashSize; i++)
        {
            if (hashBytes[i + SaltSize] != hash[i])
            {
                return false; // Passwords don't match
            }
        }
        return true; // Passwords match
    }
}