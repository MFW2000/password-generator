using MFW.PasswordGenerator.Enumerations;

namespace MFW.PasswordGenerator.Prompts.Feature;

/// <summary>
/// Responsible for assisting the user in hashing a password.
/// </summary>
public class HashPassword : Prompt
{
    /// <inheritdoc/>
    public override PromptType? DisplayMainPrompt()
    {
        Console.WriteLine("This feature is currently not available.");
        Console.WriteLine();

        ContinuePrompt();

        return PromptType.MainMenu;
    }
}
