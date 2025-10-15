using MFW.PasswordGenerator.Enumerations;
using MFW.PasswordGenerator.Helpers;
using MFW.PasswordGenerator.Infrastructure.Interfaces;
using MFW.PasswordGenerator.Providers.Interfaces;

namespace MFW.PasswordGenerator.Prompts.Main;

/// <summary>
/// Responsible for guiding the user to all application features.
/// </summary>
public class MainMenu(IAssemblyVersionProvider assemblyVersionProvider, IConsoleLogger logger) : Prompt
{
    /// <inheritdoc/>
    public override PromptType? DisplayMainPrompt()
    {
        Console.WriteLine($"=== {CommonText.AppTitle}{GetAssemblyVersion()} ===");
        Console.WriteLine(CommonText.AppSubTitle);
        Console.WriteLine();
        Console.WriteLine("--- Main Menu ---");
        Console.WriteLine($"1. {CommonText.GenerateDefaultPasswordTitle}");
        Console.WriteLine($"2. {CommonText.GenerateCustomPasswordTitle}");
        Console.WriteLine("3. Exit");
        Console.WriteLine(CommonText.TooltipOption);

        while (true)
        {
            Console.Write(CommonText.InputPrompt);

            var input = PromptHelper.ReadString(true);

            switch (input.ToLower())
            {
                case "1":
                    return PromptType.GenerateDefaultPassword;
                case "2":
                    return PromptType.GenerateCustomPassword;
                case "3":
                    return null;
                default:
                    Console.WriteLine("Please select a valid menu option.");
                    break;
            }
        }
    }

    /// <summary>
    /// Retrieves the application's assembly version in the format of "major.minor.build". The version will not be
    /// shown if it could not be retrieved.
    /// </summary>
    /// <returns>The formatted application version or an empty string if the version could not be retrieved.</returns>
    private string GetAssemblyVersion()
    {
        var version = assemblyVersionProvider.GetVersion();

        if (version is not null)
        {
            return $" v{version.ToString(3)}";
        }

        logger.LogError("Unable to retrieve the application version.");

        return string.Empty;
    }
}
