namespace MFW.PasswordGenerator.Providers.Interfaces;

/// <summary>
/// Defines a contract for retrieving the application's assembly version.
/// </summary>
public interface IAssemblyVersionProvider
{
    /// <summary>
    /// Retrieves the version of the application's assembly.
    /// </summary>
    /// <returns>The application version or null if the version could not be retrieved.</returns>
    Version? GetVersion();
}
