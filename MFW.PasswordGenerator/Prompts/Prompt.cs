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
    public abstract Prompt? HandlePrompt();
}
