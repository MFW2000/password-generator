using MFW.PasswordGenerator.Enumerations;
using MFW.PasswordGenerator.Providers.Interfaces;

namespace MFW.PasswordGenerator.Prompts.Main;

/// <summary>
/// Responsible for guiding the user to all application features.
/// </summary>
/// <param name="assemblyVersionProvider">Provides the assembly version of the application.</param>
public class MainMenu(IAssemblyVersionProvider assemblyVersionProvider) : Prompt
{
    /// <inheritdoc/>
    public override void DisplayPrompt()
    {
        Console.WriteLine($"=== {Constants.AppTitle} v{GetAssemblyVersionString()} ===");
        Console.WriteLine(Constants.AppSubTitle);
        Console.WriteLine();
        Console.WriteLine(Constants.TooltipOption);
        Console.WriteLine("1. Generate password");
        Console.WriteLine("2. Hash password");
        Console.WriteLine("3. Exit");
    }

    /// <inheritdoc/>
    public override PromptType? HandlePrompt()
    {
        while (true)
        {
            Console.Write(Constants.InputPrompt);

            var input = Console.ReadLine() ?? string.Empty;

            switch (input.ToLower())
            {
                case "1":
                    return PromptType.GeneratePassword;
                case "2":
                    return PromptType.HashPassword;
                case "3":
                    return null;
                default:
                    Console.WriteLine(Constants.InputError);
                    break;
            }
        }
    }

    /// <summary>
    /// Retrieves the application's assembly version as a formatted string.
    /// </summary>
    /// <returns>A string representing the assembly version in the format "major.minor.build".</returns>
    private string GetAssemblyVersionString()
    {
        return assemblyVersionProvider.GetVersion().ToString(3);
    }
}
