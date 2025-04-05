using MFW.PasswordGenerator.Enumerations;

namespace MFW.PasswordGenerator.Prompts;

/// <summary>
/// Defines the structure for prompts.
/// </summary>
public abstract class Prompt
{
    /// <summary>
    /// Display the prompt to the user.
    /// </summary>
    public abstract void DisplayPrompt();

    /// <summary>
    /// Handle the user's input after the prompt is displayed.
    /// </summary>
    /// <returns>Next prompt to navigate to or null to exit the application.</returns>
    public abstract PromptType? HandlePrompt();

    // TODO: These method should probably be made public for testing purposes.

    /// <summary>
    /// Displays a prompt to the user, asking whether they want to continue.
    /// </summary>
    protected static void ContinuePrompt()
    {
        Console.WriteLine(CommonText.TooltipContinue);

        Console.Write(CommonText.InputPrompt);
        Console.ReadKey();
    }

    /// <summary>
    /// Prompts the user for a yes/no response with an optional question and default answer.
    /// </summary>
    /// <param name="question">Optional question text to be displayed.</param>
    /// <param name="defaultAnswer">Optional default answer to be returned on empty input.</param>
    /// <returns>True for "yes" or "y", false for "no" or "n" (case-insensitive).</returns>
    protected static bool PromptYesNo(string? question = null, bool? defaultAnswer = null)
    {
        if (!string.IsNullOrWhiteSpace(question))
        {
            Console.WriteLine(question);
        }

        while (true)
        {
            Console.Write(CommonText.InputPrompt);

            var input = Console.ReadLine() ?? string.Empty;

            if (string.IsNullOrEmpty(input) && defaultAnswer.HasValue)
            {
                return defaultAnswer.Value;
            }

            switch (input.ToLower())
            {
                case "yes" or "y":
                    return true;
                case "no" or "n":
                    return false;
            }

            Console.WriteLine(CommonText.InputError);
        }
    }
}
