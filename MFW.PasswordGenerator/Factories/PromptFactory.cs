using MFW.PasswordGenerator.Factories.Interfaces;
using MFW.PasswordGenerator.Prompts;
using Microsoft.Extensions.DependencyInjection;

namespace MFW.PasswordGenerator.Factories;

public class PromptFactory(IServiceProvider serviceProvider) : IPromptFactory
{
    public T CreatePrompt<T>() where T : Prompt
    {
        return serviceProvider.GetRequiredService<T>();
    }
}
