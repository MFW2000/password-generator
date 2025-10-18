using MFW.PasswordGenerator.Factories.Interfaces;
using MFW.PasswordGenerator.Presentation;
using Microsoft.Extensions.DependencyInjection;

namespace MFW.PasswordGenerator.Factories;

/// <summary>
/// Implements <see cref="IPromptFactory"/> for instantiating <see cref="Prompt"/> objects with support for
/// dependency injection.
/// </summary>
/// <param name="serviceProvider">
/// The service provider used to resolve and create instances of <see cref="Prompt"/> objects.
/// </param>
public class PromptFactory(IServiceProvider serviceProvider) : IPromptFactory
{
    /// <inheritdoc/>
    public T CreatePrompt<T>() where T : Prompt
    {
        return serviceProvider.GetRequiredService<T>();
    }
}
