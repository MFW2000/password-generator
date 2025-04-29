namespace MFW.PasswordGenerator.Helpers;

// TODO: Implement tests

/// <summary>
/// Provides helper methods for handling console input and other prompt-related functionalities.
/// </summary>
public static class PromptHelpers
{
    public static string ReadString(bool allowEmpty = false, bool trim = true, int? maxLength = null)
    {
        var input = Console.ReadLine() ?? string.Empty;

        if (!allowEmpty && string.IsNullOrWhiteSpace(input))
        {
            throw new ArgumentException("Input cannot be empty.");
        }

        if (trim)
        {
            input = input.Trim();
        }

        if (input.Length > maxLength)
        {
            throw new ArgumentOutOfRangeException(input, "Input is too long.");
        }

        return input;
    }

    public static int ReadInt(int? minRange = null, int? maxRange = null)
    {
        var input = ReadString();

        if (!int.TryParse(input, out var result))
        {
            throw new FormatException("Input is not a valid integer.");
        }

        if (result < minRange)
        {
            throw new ArgumentOutOfRangeException(input, "Input is less than the minimum range.");
        }

        if (result > maxRange)
        {
            throw new ArgumentOutOfRangeException(input, "Input is greater than the maximum range.");
        }

        return result;
    }
}
