using MFW.PasswordGenerator.Prompts.Main;

namespace MFW.PasswordGenerator.Prompts.Feature;

/// <summary>
/// Responsible for assisting the user in hashing a password.
/// </summary>
public class HashPassword : Prompt
{
    // TODO: Implement feature.

    public override void DisplayPrompt()
    {
        Console.WriteLine("This is the password hash feature.");
        Console.WriteLine();
    }

    public override Prompt HandlePrompt()
    {
        ContinuePrompt();

        return new MainMenu();
    }
}
