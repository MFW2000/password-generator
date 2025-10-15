using System.Reflection;
using MFW.PasswordGenerator.Providers.Interfaces;

namespace MFW.PasswordGenerator.Providers;

/// <summary>
/// Implements <see cref="IAssemblyVersionProvider"/> for retrieving the application's assembly version.
/// </summary>
/// <param name="assembly">The assembly whose version is retrieved.</param>
public class AssemblyVersionProvider(Assembly assembly) : IAssemblyVersionProvider
{
    /// <inheritdoc/>
    public Version? GetVersion()
    {
        return assembly.GetName().Version;
    }
}
