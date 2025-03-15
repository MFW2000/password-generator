using MFW.PasswordGenerator.Core.Interfaces;
using MFW.PasswordGenerator.Enumerations;
using MFW.PasswordGenerator.Factories.Interfaces;
using MFW.PasswordGenerator.Prompts;
using MFW.PasswordGenerator.Prompts.Feature;
using MFW.PasswordGenerator.Prompts.Main;

namespace MFW.PasswordGenerator.Core;

/// <summary>
/// Provides the main execution loop for the Password Generator application.
/// </summary>
public class PromptRunner(IPromptFactory promptFactory) : IPromptRunner
{
    /// <inheritdoc/>
    public void Run()
    {
        Prompt? currentPrompt = promptFactory.CreatePrompt<MainMenu>();

        while (currentPrompt is not null)
        {
            // TODO: Method can currently not be tested, Console might have to be abstracted so it can be mocked.
            Console.Clear();

            currentPrompt.DisplayPrompt();

            var promptResult = currentPrompt.HandlePrompt();

            currentPrompt = GetNextPrompt(promptResult);
        }
    }

    private Prompt? GetNextPrompt(PromptType? promptType)
    {
        return promptType switch
        {
            PromptType.MainMenu => promptFactory.CreatePrompt<MainMenu>(),
            PromptType.GeneratePassword => promptFactory.CreatePrompt<GeneratePassword>(),
            PromptType.HashPassword => promptFactory.CreatePrompt<HashPassword>(),
            _ => null
        };
    }
}
