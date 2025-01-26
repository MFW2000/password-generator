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

    /// <summary>
    /// Displays a prompt to the user, asking whether they want to continue.
    /// </summary>
    protected static void ContinuePrompt()
    {
        Console.WriteLine(Constants.TooltipContinue);

        Console.Write(Constants.InputPrompt);
        Console.ReadKey();
    }
}
