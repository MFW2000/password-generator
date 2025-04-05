using MFW.PasswordGenerator.Enumerations;
using MFW.PasswordGenerator.Records;
using MFW.PasswordGenerator.Services.Interfaces;

namespace MFW.PasswordGenerator.Prompts.Feature;

/// <summary>
/// Responsible for assisting the user in generating a new password.
/// </summary>
public class GeneratePassword(IPasswordGeneratorService passwordGeneratorService) : Prompt
{
    /// <inheritdoc/>
    public override void DisplayPrompt()
    {
        Console.WriteLine("Enter the length of the password (default 18):");

        Console.WriteLine("Include upper case characters (yes/no, default yes):");

        Console.WriteLine("Include lower case characters (yes/no, default yes):");

        Console.WriteLine("Enter the minimum amount of numbers (default 1):");

        Console.WriteLine("Enter the minimum amount of special characters (default 1):");

        Console.WriteLine("Avoid ambiguous characters (yes/no, default no):");

        var options = new PasswordGeneratorOptions(18, true, true, 1, 1, true);

        var password = passwordGeneratorService.Generate(options);

        Console.WriteLine(password);
    }

    /// <inheritdoc/>
    public override PromptType? HandlePrompt()
    {
        ContinuePrompt();

        return PromptType.MainMenu;
    }
}
