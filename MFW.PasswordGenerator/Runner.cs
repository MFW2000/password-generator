using MFW.PasswordGenerator.Enumerations;
using MFW.PasswordGenerator.Factories.Interfaces;
using MFW.PasswordGenerator.Infrastructure.Interfaces;
using MFW.PasswordGenerator.Presentation;
using MFW.PasswordGenerator.Presentation.Feature;
using MFW.PasswordGenerator.Presentation.Main;

namespace MFW.PasswordGenerator;

/// <summary>
/// Provides the main execution loop for the Password Generator application.
/// </summary>
/// <param name="promptFactory">The factory used to create new instances of prompts.</param>
/// <param name="consoleClear">Provides console clearing functionality that can be tested.</param>
public class Runner(IPromptFactory promptFactory, IConsoleClear consoleClear)
{
    /// <summary>
    /// Executes the main loop of the application.
    /// </summary>
    public void Run()
    {
        Prompt? currentPrompt = promptFactory.CreatePrompt<MainMenu>();

        while (currentPrompt is not null)
        {
            consoleClear.Clear();

            var promptResult = currentPrompt.DisplayMainPrompt();

            currentPrompt = GetNextPrompt(promptResult);
        }
    }

    /// <summary>
    /// Retrieves the next <see cref="Prompt"/> instance based on the specified prompt type.
    /// </summary>
    /// <param name="promptType">
    /// The <see cref="PromptType"/> value that determines which <see cref="Prompt"/> to create,
    /// or null if no specific prompt is requested.
    /// </param>
    /// <returns>
    /// A new <see cref="Prompt"/> instance corresponding to the specified <see cref="PromptType"/>,
    /// or null if the prompt type is unrecognized or null.
    /// </returns>
    private Prompt? GetNextPrompt(PromptType? promptType)
    {
        return promptType switch
        {
            PromptType.MainMenu => promptFactory.CreatePrompt<MainMenu>(),
            PromptType.GenerateDefaultPassword => promptFactory.CreatePrompt<GenerateDefaultPassword>(),
            PromptType.GenerateCustomPassword => promptFactory.CreatePrompt<GenerateCustomPassword>(),
            _ => null
        };
    }
}
