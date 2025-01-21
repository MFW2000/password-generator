using MFW.PasswordGenerator.Prompts;
using MFW.PasswordGenerator.Prompts.Main;

namespace MFW.PasswordGenerator;

/// <summary>
/// Provides the main execution loop for the Password Generator application.
/// </summary>
public static class PasswordGeneratorRunner
{
    private static Prompt? _currentPrompt = new MainMenu();

    /// <summary>
    /// Executes the main loop of the Password Generator application.
    /// </summary>
    public static void Run()
    {
        while (_currentPrompt is not null)
        {
            Console.Clear();

            _currentPrompt.DisplayPrompt();
            _currentPrompt = _currentPrompt.HandlePrompt();
        }
    }
}
