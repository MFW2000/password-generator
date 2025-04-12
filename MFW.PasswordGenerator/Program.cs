using MFW.PasswordGenerator.Factories;
using MFW.PasswordGenerator.Factories.Interfaces;
using MFW.PasswordGenerator.Infrastructure;
using MFW.PasswordGenerator.Infrastructure.Interfaces;
using MFW.PasswordGenerator.Prompts.Feature;
using MFW.PasswordGenerator.Prompts.Main;
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
        var runner = serviceProvider.GetRequiredService<PromptRunner>();

        runner.Run();
    }

    /// <summary>
    /// Configures and builds a service provider with registered dependency injection services for the application.
    /// </summary>
    /// <returns>A <see cref="ServiceProvider"/> instance containing the configured services.</returns>
    private static ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        // Register services.
        services.AddSingleton<IAssemblyVersionProvider>(_ => new AssemblyVersionProvider(typeof(Program).Assembly));
        services.AddTransient<IPromptFactory, PromptFactory>();
        services.AddTransient<IConsoleClear, ConsoleClear>();
        services.AddTransient<IPasswordGeneratorService, PasswordGeneratorService>();

        // Register application loop runner.
        services.AddTransient<PromptRunner>();

        // Register prompts.
        services.AddTransient<MainMenu>();
        services.AddTransient<GeneratePassword>();
        services.AddTransient<HashPassword>();

        // Register external services.
        services.InjectClipboard();

        return services.BuildServiceProvider();
    }
}
