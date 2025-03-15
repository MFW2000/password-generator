using MFW.PasswordGenerator.Prompts;

namespace MFW.PasswordGenerator.Factories.Interfaces;

public interface IPromptFactory
{
    T CreatePrompt<T>() where T : Prompt;
}
