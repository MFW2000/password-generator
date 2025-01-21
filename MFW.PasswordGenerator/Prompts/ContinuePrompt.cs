namespace MFW.PasswordGenerator.Prompts;

/// <summary>
/// Responsible for displaying a prompt to the user, asking whether they want to continue.
/// </summary>
public static class ContinuePrompt
{
    /// <summary>
    /// Displays the continue prompt to the user and waits for the user's key press.
    /// </summary>
    public static void Display()
    {
        Console.WriteLine(Constants.TooltipContinue);

        Console.Write(Constants.InputPrompt);
        Console.ReadKey();
    }
}
