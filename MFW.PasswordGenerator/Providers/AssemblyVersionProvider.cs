using MFW.PasswordGenerator.Providers.Interfaces;

namespace MFW.PasswordGenerator.Providers;

public class AssemblyVersionProvider : IAssemblyVersionProvider
{
    public Version GetVersion()
    {
        var version = typeof(Program).Assembly.GetName().Version;
        
        return version ?? new Version(0, 0, 0);
    }
}
