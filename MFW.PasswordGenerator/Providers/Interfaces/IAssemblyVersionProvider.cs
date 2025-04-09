namespace MFW.PasswordGenerator.Providers.Interfaces;

/// <summary>
/// Defines a contract for retrieving the application's assembly version.
/// </summary>
public interface IAssemblyVersionProvider
{
    /// <summary>
    /// Retrieves the version of the application's assembly.
    /// </summary>
    /// <returns>A <see cref="Version"/> object representing the assembly's version.</returns>
    Version GetVersion();
}
