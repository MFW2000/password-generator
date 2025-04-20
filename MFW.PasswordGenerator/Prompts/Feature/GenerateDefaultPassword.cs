using MFW.PasswordGenerator.Enumerations;
using MFW.PasswordGenerator.Records;
using MFW.PasswordGenerator.Services.Interfaces;
using TextCopy;

namespace MFW.PasswordGenerator.Prompts.Feature;

/// <summary>
/// Responsible for assisting the user in quickly generating a new password with default secure settings.
/// </summary>
public class GenerateDefaultPassword(IPasswordGeneratorService passwordGeneratorService, IClipboard clipboard) : Prompt
{
    /// <inheritdoc/>
    public override PromptType? DisplayMainPrompt()
    {
        Console.WriteLine($"=== {CommonText.GenerateDefaultPasswordTitle} ===");
        Console.WriteLine("Generate a new password with default secure settings.");
        Console.WriteLine();

        var options = new PasswordGeneratorOptions(
            Constants.PasswordLengthDefault,
            Constants.UseUppercaseInPasswordDefault,
            Constants.UseLowercaseInPasswordDefault,
            Constants.MinimumPasswordDigitsDefault,
            Constants.MinimumSpecialPasswordCharactersDefault,
            Constants.AvoidAmbiguousCharactersInPasswordDefault);

        string password;

        try
        {
            password = passwordGeneratorService.Generate(options);
        }
        catch (Exception)
        {
            Console.WriteLine("An error occurred while generating the password.");

            ContinuePrompt();

            return PromptType.MainMenu;
        }

        Console.WriteLine("Generating password...");
        Console.WriteLine();
        Console.WriteLine($"New password: {password}");

        try
        {
            clipboard.SetText(password);

            Console.WriteLine("The password was saved to your clipboard.");
        }
        catch (Exception)
        {
            Console.WriteLine(
                "The password could not be saved to your clipboard, make sure you have the correct " +
                "dependencies installed.");
        }

        Console.WriteLine();

        ContinuePrompt();

        return PromptType.MainMenu;
    }
}
