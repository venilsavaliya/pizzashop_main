using BCrypt.Net;

public class PasswordUtility
{
    // Hash a password
    public static string HashPassword(string plainPassword)
    {
        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword);
        Console.WriteLine(hashedPassword);
        return hashedPassword;
    }

    // Verify if the password matches the hashed password
    public static bool VerifyPassword(string plainPassword, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(plainPassword, hashedPassword);
    }
}
