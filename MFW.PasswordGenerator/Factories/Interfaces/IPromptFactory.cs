using MFW.PasswordGenerator.Presentation;

namespace MFW.PasswordGenerator.Factories.Interfaces;

/// <summary>
/// Defines a contract for instantiating <see cref="Prompt"/> objects with support for dependency injection.
/// </summary>
public interface IPromptFactory
{
    /// <summary>
    /// Creates a new instance of a specified <see cref="Prompt"/> type.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="Prompt"/> to create.</typeparam>
    /// <returns>A new instance of the specified <see cref="Prompt"/> type.</returns>
    T CreatePrompt<T>() where T : Prompt;
}
