﻿using MFW.PasswordGenerator.Enumerations;
using MFW.PasswordGenerator.Records;
using MFW.PasswordGenerator.Services.Interfaces;
using TextCopy;

namespace MFW.PasswordGenerator.Prompts.Feature;

/// <summary>
/// Responsible for assisting the user in generating a new password.
/// </summary>
public class GeneratePassword(IPasswordGeneratorService passwordGeneratorService, IClipboard clipboard) : Prompt
{
    /// <inheritdoc/>
    public override PromptType? DisplayMainPrompt()
    {
        Console.WriteLine("=== Generate Password ===");
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

        var password = passwordGeneratorService.Generate(options);

        clipboard.SetText(password);

        Console.WriteLine("Generating password...");
        Console.WriteLine();
        Console.WriteLine($"New password: {password}");
        Console.WriteLine("The password was saved to your clipboard.");
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
            Console.Write(CommonText.InputPrompt);

            var input = Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrEmpty(input))
            {
                return Constants.PasswordLengthDefault;
            }

            if (!int.TryParse(input, out var length))
            {
                Console.WriteLine("Please enter a valid digit.");

                continue;
            }

            if (length is < Constants.MinimumPasswordLength or > Constants.MaximumPasswordLength)
            {
                Console.WriteLine("Please enter a valid length.");

                continue;
            }

            return length;
        }
    }

    /// <summary>
    /// Prompts the user to enter the minimum valid amount of digits or nothing to set the default amount of digits.
    /// </summary>
    /// <param name="length">The password length to validate the result.</param>
    /// <returns>The specified amount of digits or the default amount when the input is empty.</returns>
    private static int PromptMinimumDigits(int length)
    {
        Console.WriteLine(
            $"Enter the minimum amount of digits to be included (default {Constants.MinimumPasswordDigitsDefault}):");

        while (true)
        {
            Console.Write(CommonText.InputPrompt);

            var input = Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrEmpty(input))
            {
                return Constants.MinimumPasswordDigitsDefault;
            }

            if (!int.TryParse(input, out var minimumDigits))
            {
                Console.WriteLine("Please enter a valid digit.");

                continue;
            }

            if (minimumDigits > length)
            {
                Console.WriteLine("The minimum amount of digits cannot exceed the length of the password.");

                continue;
            }

            return minimumDigits;
        }
    }

    /// <summary>
    /// Prompts the user to enter the minimum valid amount of special characters or nothing to set the default
    /// amount of special characters.
    /// </summary>
    /// <param name="length">The password length to validate the result.</param>
    /// <param name="minimumDigits">The minimum amount of digits to validate the result.</param>
    /// <returns>The specified amount of special characters or the default amount when the input is empty.</returns>
    private static int PromptMinimumSpecialCharacters(int length, int minimumDigits)
    {
        if (length == minimumDigits)
        {
            Console.WriteLine(
                "Special character input skipped, there is no room left for additional special characters.");

            return 0;
        }

        Console.WriteLine(
            $"Enter the minimum amount of special characters to be included " +
            $"(default {Constants.MinimumSpecialPasswordCharactersDefault}):");

        while (true)
        {
            Console.Write(CommonText.InputPrompt);

            var input = Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrEmpty(input))
            {
                return Constants.MinimumSpecialPasswordCharactersDefault;
            }

            if (!int.TryParse(input, out var minimumSpecialCharacters))
            {
                Console.WriteLine("Please enter a valid digit.");

                continue;
            }

            if (minimumDigits + minimumSpecialCharacters > length)
            {
                Console.WriteLine(
                    "The combined total of special characters and digits cannot exceed the password's length.");

                continue;
            }

            return minimumSpecialCharacters;
        }
    }
}
