using MFW.PasswordGenerator.Enumerations;

namespace MFW.PasswordGenerator.Prompts.Feature;

/// <summary>
/// Responsible for assisting the user in quickly generating a new password with default secure settings.
/// </summary>
public class GenerateDefaultPassword : Prompt
{
    /// <inheritdoc/>
    public override PromptType? DisplayMainPrompt()
    {
        return PromptType.MainMenu;
    }
}
