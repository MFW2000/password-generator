using MFW.PasswordGenerator.Factories;
using MFW.PasswordGenerator.Factories.Interfaces;
using MFW.PasswordGenerator.Infrastructure;
using MFW.PasswordGenerator.Infrastructure.Interfaces;
using MFW.PasswordGenerator.Presentation.Feature;
using MFW.PasswordGenerator.Presentation.Main;
using MFW.PasswordGenerator.Providers;
using MFW.PasswordGenerator.Providers.Interfaces;
using MFW.PasswordGenerator.Services;
using MFW.PasswordGenerator.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using TextCopy;

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
        var serviceProvider = ConfigureServices();
        var runner = serviceProvider.GetRequiredService<Runner>();

        runner.Run();
    }

    /// <summary>
    /// Configures and builds a service provider with registered dependency injection services for the application.
    /// </summary>
    /// <returns>A <see cref="ServiceProvider"/> instance containing the configured services.</returns>
    private static ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        // Register core framework services.
        services.AddSingleton(TimeProvider.System);

        // Register pre-configured services using direct instance registration.
        services.AddSingleton<IAssemblyVersionProvider>(new AssemblyVersionProvider(typeof(Program).Assembly));

        // Register services.
        services.AddSingleton<IConsoleLogger, ConsoleLogger>();
        services.AddTransient<IConsoleClear, ConsoleClear>();
        services.AddTransient<IPromptFactory, PromptFactory>();
        services.AddTransient<IPasswordGeneratorService, PasswordGeneratorService>();

        // Register runner service to manage application loop.
        services.AddTransient<Runner>();

        // Register prompts.
        services.AddTransient<MainMenu>();
        services.AddTransient<GenerateDefaultPassword>();
        services.AddTransient<GenerateCustomPassword>();

        // Register third-party services.
        services.InjectClipboard();

        return services.BuildServiceProvider();
    }
}
