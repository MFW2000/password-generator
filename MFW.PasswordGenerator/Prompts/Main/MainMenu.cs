using MFW.PasswordGenerator.Enumerations;
using MFW.PasswordGenerator.Helpers;
using MFW.PasswordGenerator.Providers.Interfaces;

namespace MFW.PasswordGenerator.Prompts.Main;

/// <summary>
/// Responsible for guiding the user to all application features.
/// </summary>
/// <param name="assemblyVersionProvider">Provides the assembly version of the application.</param>
public class MainMenu(IAssemblyVersionProvider assemblyVersionProvider) : Prompt
{
    /// <inheritdoc/>
    public override PromptType? DisplayMainPrompt()
    {
        Console.WriteLine($"=== {CommonText.AppTitle} v{GetAssemblyVersionString()} ===");
        Console.WriteLine(CommonText.AppSubTitle);
        Console.WriteLine();
        Console.WriteLine("--- Main Menu ---");
        Console.WriteLine($"1. {CommonText.GenerateDefaultPasswordTitle}");
        Console.WriteLine($"2. {CommonText.GenerateCustomPasswordTitle}");
        Console.WriteLine($"3. {CommonText.HashPasswordTitle}");
        Console.WriteLine("4. Exit");
        Console.WriteLine(CommonText.TooltipOption);

        while (true)
        {
            Console.Write(CommonText.InputPrompt);

            var input = PromptHelpers.ReadString();

            switch (input.ToLower())
            {
                case "1":
                    return PromptType.GenerateDefaultPassword;
                case "2":
                    return PromptType.GenerateCustomPassword;
                case "3":
                    return PromptType.HashPassword;
                case "4":
                    return null;
                default:
                    Console.WriteLine("Please select a valid menu option number.");
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
