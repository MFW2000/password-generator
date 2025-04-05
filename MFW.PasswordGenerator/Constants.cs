namespace MFW.PasswordGenerator;

/// <summary>
/// Centralized character set constants for consistency across the project.
/// </summary>
public static class Constants
{
    public const string Uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public const string Lowercase = "abcdefghijklmnopqrstuvwxyz";
    public const string Digits = "0123456789";
    public const string Special = "!@#$%^&*";
    public const string AmbiguousCharacters = "lIO01";

    public const int MinimumPasswordLength = 5;
    public const int MaximumPasswordLength = 128;
}
