namespace MFW.PasswordGenerator.Helpers;

/// <summary>
/// Provides helper methods for handling console input and other prompt-related functionalities.
/// </summary>
public static class PromptHelpers
{
    /// <summary>
    /// Reads a line from the console and returns it trimmed or empty if the input is null.
    /// </summary>
    /// <returns>Trimmed input string or empty string if input is null.</returns>
    public static string ReadTrimmedLine()
    {
        return (Console.ReadLine() ?? string.Empty).Trim();
    }
}
