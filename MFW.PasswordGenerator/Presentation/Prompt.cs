using MFW.PasswordGenerator.Enumerations;
using MFW.PasswordGenerator.Helpers;

namespace MFW.PasswordGenerator.Presentation;

/// <summary>
/// Defines the structure for prompts.
/// </summary>
public abstract class Prompt
{
    /// <summary>
    /// Display the main prompt and handle the user's input.
    /// </summary>
    /// <returns>Next prompt to navigate to or null to exit the application.</returns>
    public abstract PromptType? DisplayMainPrompt();

    /// <summary>
    /// Displays a prompt to the user, asking whether they want to continue.
    /// </summary>
    protected static void ContinuePrompt()
    {
        Console.WriteLine(CommonText.TooltipContinue);

        Console.Write(CommonText.InputPrompt);
        Console.ReadLine();
    }

    /// <summary>
    /// Prompts the user for a yes/no response with optional prompt text and default value.
    /// </summary>
    /// <param name="promptText">Optional prompt text to be displayed.</param>
    /// <param name="defaultValue">Optional default value to be returned on empty input.</param>
    /// <returns>True for "yes" or "y", false for "no" or "n" (case-insensitive).</returns>
    protected static bool PromptYesNo(string? promptText = null, bool? defaultValue = null)
    {
        if (!string.IsNullOrWhiteSpace(promptText))
        {
            Console.WriteLine(promptText);
        }

        while (true)
        {
            Console.Write(CommonText.InputPrompt);

            var input = PromptHelper.ReadString(true);

            if (string.IsNullOrEmpty(input) && defaultValue.HasValue)
            {
                return defaultValue.Value;
            }

            switch (input.ToLower())
            {
                case "yes" or "y":
                    return true;
                case "no" or "n":
                    return false;
            }

            Console.WriteLine("Please enter 'yes' (y) or 'no' (n).");
        }
    }
}
