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
        Console.WriteLine("Generate a new password with default secure settings");

        var options = new PasswordGeneratorOptions(
            Constants.PasswordLengthDefault,
            Constants.UseUppercaseInPasswordDefault,
            Constants.UseLowercaseInPasswordDefault,
            Constants.MinimumPasswordDigitsDefault,
            Constants.MinimumSpecialPasswordCharactersDefault,
            Constants.AvoidAmbiguousCharactersInPasswordDefault);

        return PromptType.MainMenu;
    }
}
