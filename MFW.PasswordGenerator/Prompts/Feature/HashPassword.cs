using MFW.PasswordGenerator.Enumerations;

namespace MFW.PasswordGenerator.Prompts.Feature;

/// <summary>
/// Responsible for assisting the user in hashing a password.
/// </summary>
public class HashPassword : Prompt
{
    // TODO: Implement feature.

    /// <inheritdoc/>
    public override void DisplayPrompt()
    {
        Console.WriteLine("This is the password hash feature.");
        Console.WriteLine();
    }

    /// <inheritdoc/>
    public override PromptType? HandlePrompt()
    {
        ContinuePrompt();

        return PromptType.MainMenu;
    }
}
