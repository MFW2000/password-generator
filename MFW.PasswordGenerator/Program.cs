namespace MFW.PasswordGenerator;

/// <summary>
/// Represents the primary entry point for running the application.
/// </summary>
public static class Program
{
    /// <summary>
    /// The entry point of the application.
    /// </summary>
    public static void Main()
    {
        PasswordGeneratorRunner.Run();
    }

    /// <summary>
    /// Retrieves the version of the application from the assembly metadata.
    /// </summary>
    /// <returns>The application version in the format "Major.Minor.Build"</returns>
    public static string GetApplicationVersion()
    {
        var version = typeof(Program).Assembly.GetName().Version;

        return version is null ? "0.0.0" : $"{version.Major}.{version.Minor}.{version.Build}";
    }
}
