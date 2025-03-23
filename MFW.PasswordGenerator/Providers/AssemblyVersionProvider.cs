using MFW.PasswordGenerator.Providers.Interfaces;

namespace MFW.PasswordGenerator.Providers;

/// <summary>
/// Implements <see cref="IAssemblyVersionProvider"/> for retrieving the application's assembly version.
/// </summary>
public class AssemblyVersionProvider : IAssemblyVersionProvider
{
    /// <inheritdoc/>
    public Version GetVersion()
    {
        var version = typeof(Program).Assembly.GetName().Version;

        return version ?? new Version(0, 0, 0);
    }
}
