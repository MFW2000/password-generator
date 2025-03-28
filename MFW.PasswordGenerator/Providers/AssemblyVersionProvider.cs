using System.Reflection;
using MFW.PasswordGenerator.Providers.Interfaces;

namespace MFW.PasswordGenerator.Providers;

/// <summary>
/// Implements <see cref="IAssemblyVersionProvider"/> for retrieving the application's assembly version.
/// </summary>
/// <param name="assembly">The assembly whose version is retrieved.</param>
/// <remarks>
/// The <paramref name="assembly"/> is guaranteed non-null at runtime when provided via dependency injection.
/// </remarks>
public class AssemblyVersionProvider(Assembly assembly) : IAssemblyVersionProvider
{
    private readonly Assembly _assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));

    /// <inheritdoc/>
    public Version GetVersion()
    {
        var version = _assembly.GetName().Version;

        return version ?? new Version(0, 0, 0);
    }
}
