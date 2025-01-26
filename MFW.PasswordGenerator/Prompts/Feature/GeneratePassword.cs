﻿using MFW.PasswordGenerator.Prompts.Main;

namespace MFW.PasswordGenerator.Prompts.Feature;

/// <summary>
/// Responsible for assisting the user in generating a new password.
/// </summary>
public class GeneratePassword : Prompt
{
    // TODO: Implement feature.

    /// <inheritdoc/>
    public override void DisplayPrompt()
    {
        Console.WriteLine("This is the password generator feature.");
        Console.WriteLine();
    }

    /// <inheritdoc/>
    public override Prompt HandlePrompt()
    {
        ContinuePrompt();

        return new MainMenu();
    }
}
