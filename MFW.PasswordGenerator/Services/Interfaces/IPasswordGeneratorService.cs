using MFW.PasswordGenerator.Exceptions;
using MFW.PasswordGenerator.Records;

namespace MFW.PasswordGenerator.Services.Interfaces;

/// <summary>
/// Defines a contract for generating new passwords.
/// </summary>
public interface IPasswordGeneratorService
{
    /// <summary>
    /// Validates and uses the specified options to generate a new randomized password.
    /// </summary>
    /// <param name="options">The options that specify the password requirements.</param>
    /// <returns>The new randomized password.</returns>
    /// <exception cref="PasswordGeneratorException">Thrown when invalid options are supplied.</exception>
    string Generate(PasswordGeneratorOptions options);
}
