using MFW.PasswordGenerator.Enumerations;
using MFW.PasswordGenerator.Helpers;
using MFW.PasswordGenerator.Records;
using MFW.PasswordGenerator.Services.Interfaces;
using TextCopy;

namespace MFW.PasswordGenerator.Prompts.Feature;

/// <summary>
/// Responsible for assisting the user in generating a new customized password.
/// </summary>
public class GenerateCustomPassword(IPasswordGeneratorService passwordGeneratorService, IClipboard clipboard) : Prompt
{
    /// <inheritdoc/>
    public override PromptType? DisplayMainPrompt()
    {
        Console.WriteLine($"=== {CommonText.GenerateCustomPasswordTitle} ===");
        Console.WriteLine("Generate a new password with the preferences of your choice.");
        Console.WriteLine();
        Console.WriteLine("--- Constraints ---");
        Console.WriteLine("The password must comply with the following constraints:");
        Console.WriteLine(
            $"- Length must be between {Constants.MinimumPasswordLength} and " +
            $"{Constants.MaximumPasswordLength} characters");
        Console.WriteLine(
            "- The password must at least contain uppercases, lowercases, digits, or special characters");
        Console.WriteLine("- There may not be more digits and/or special characters than the length of the password");
        Console.WriteLine();
        Console.WriteLine("--- Preferences ---");

        var length = PromptPasswordLength();
        var includeUppercase = PromptYesNo(
            "Include uppercase characters (YES/no):",
            Constants.UseUppercaseInPasswordDefault);
        var includeLowercase = PromptYesNo(
            "Include lowercase characters (YES/no):",
            Constants.UseLowercaseInPasswordDefault);
        var minimumDigits = PromptMinimumDigits(length);
        var minimumSpecialCharacters = PromptMinimumSpecialCharacters(length, minimumDigits);
        var avoidAmbiguousCharacters = PromptYesNo(
            "Avoid ambiguous characters (yes/NO):",
            Constants.AvoidAmbiguousCharactersInPasswordDefault);

        var options = new PasswordGeneratorOptions(
            length,
            includeUppercase,
            includeLowercase,
            minimumDigits,
            minimumSpecialCharacters,
            avoidAmbiguousCharacters);

        if (!IsValidPasswordConfig(includeUppercase, includeLowercase, minimumDigits, minimumSpecialCharacters))
        {
            Console.WriteLine(
                "Password options must include at least one of: uppercase letters, lowercase letters, digits, " +
                "or special characters.");
            Console.ReadLine();

            return PromptType.MainMenu;
        }

        string password;

        try
        {
            password = passwordGeneratorService.Generate(options);
        }
        catch (Exception)
        {
            Console.WriteLine("An error occurred while generating the password.");

            ContinuePrompt();

            return PromptType.MainMenu;
        }

        Console.WriteLine("Generating password...");
        Console.WriteLine();
        Console.WriteLine($"New password: {password}");

        try
        {
            clipboard.SetText(password);

            Console.WriteLine("The password was saved to your clipboard.");
        }
        catch (Exception)
        {
            Console.WriteLine(
                "The password could not be saved to your clipboard, make sure you have the correct " +
                "dependencies installed.");
        }

        Console.WriteLine();

        ContinuePrompt();

        return PromptType.MainMenu;
    }

    /// <summary>
    /// Prompts the user to enter a valid password length or nothing to set the default length.
    /// </summary>
    /// <returns>The specified length or the default length when the input is empty.</returns>
    private static int PromptPasswordLength()
    {
        Console.WriteLine($"Enter the length of the password (default {Constants.PasswordLengthDefault}):");

        while (true)
        {
            int? input;

            Console.Write(CommonText.InputPrompt);

            try
            {
                input = PromptHelper.ReadInt(
                    true,
                    Constants.MinimumPasswordLength,
                    Constants.MaximumPasswordLength);
            }
            catch (Exception)
            {
                Console.WriteLine(
                    $"The password length must be a number between {Constants.MinimumPasswordLength} " +
                    $"and {Constants.MaximumPasswordLength}.");

                continue;
            }

            return input ?? Constants.PasswordLengthDefault;
        }
    }

    /// <summary>
    /// Prompts the user to enter the minimum valid number of digits or nothing to set the default number of digits.
    /// </summary>
    /// <param name="length">The password length to validate the result.</param>
    /// <returns>The specified number of digits or the default amount when the input is empty.</returns>
    private static int PromptMinimumDigits(int length)
    {
        Console.WriteLine(
            $"Enter the minimum number of digits to be included (default {Constants.MinimumPasswordDigitsDefault}):");

        while (true)
        {
            int? input;

            Console.Write(CommonText.InputPrompt);

            try
            {
                input = PromptHelper.ReadInt(true, 0, length);
            }
            catch (Exception)
            {
                Console.WriteLine(
                    "The minimum number of digits must be a non-negative number and cannot " +
                    "exceed the password length.");

                continue;
            }

            return input ?? Constants.MinimumPasswordDigitsDefault;
        }
    }

    /// <summary>
    /// Prompts the user to enter the minimum valid number of special characters or nothing to set the default
    /// number of special characters.
    /// </summary>
    /// <param name="length">The password length to validate the result.</param>
    /// <param name="minimumDigits">The minimum number of digits to validate the result.</param>
    /// <returns>The specified number of special characters or the default amount when the input is empty.</returns>
    private static int PromptMinimumSpecialCharacters(int length, int minimumDigits)
    {
        if (length == minimumDigits)
        {
            Console.WriteLine(
                "Special character input skipped, there is no room left for additional special characters.");

            return 0;
        }

        Console.WriteLine(
            $"Enter the minimum number of special characters to be included " +
            $"(default {Constants.MinimumSpecialPasswordCharactersDefault}):");

        while (true)
        {
            int? input;

            Console.Write(CommonText.InputPrompt);

            try
            {
                input = PromptHelper.ReadInt(true, 0, length - minimumDigits);
            }
            catch (Exception)
            {
                Console.WriteLine(
                    "The minimum number of special characters must be a non-negative number and the combined " +
                    "total of special characters and digits cannot exceed the password's length.");

                continue;
            }

            return input ?? Constants.MinimumSpecialPasswordCharactersDefault;
        }
    }

    /// <summary>
    /// Determines whether the specified password configuration is valid.
    /// A password configuration is valid when at least one of the properties is valid.
    /// </summary>
    /// <param name="includeUppercase">Whether uppercase letters are included in the password.</param>
    /// <param name="includeLowercase">Whether lowercase letters are included in the password.</param>
    /// <param name="minimumDigits">The minimum number of digits to include in the password.</param>
    /// <param name="minimumSpecialCharacters">
    /// The minimum number of special characters to include in the password.
    /// </param>
    /// <returns>True if the password configuration is valid, false otherwise.</returns>
    private static bool IsValidPasswordConfig(
        bool includeUppercase,
        bool includeLowercase,
        int minimumDigits,
        int minimumSpecialCharacters)
    {
        return includeUppercase || includeLowercase || minimumDigits != 0 || minimumSpecialCharacters != 0;
    }
}
