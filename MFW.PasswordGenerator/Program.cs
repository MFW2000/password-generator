using MFW.PasswordGenerator.Factories;
using MFW.PasswordGenerator.Factories.Interfaces;
using MFW.PasswordGenerator.Infrastructure;
using MFW.PasswordGenerator.Infrastructure.Interfaces;
using MFW.PasswordGenerator.Prompts.Feature;
using MFW.PasswordGenerator.Prompts.Main;
using MFW.PasswordGenerator.Providers;
using MFW.PasswordGenerator.Providers.Interfaces;
using Microsoft.Extensions.DependencyInjection;

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

    private static ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        // Register services.
        services.AddTransient<IAssemblyVersionProvider, AssemblyVersionProvider>();
        services.AddTransient<IPromptFactory, PromptFactory>();
        services.AddTransient<IConsoleClear, ConsoleClear>();
        services.AddTransient<PromptRunner>();
        services.AddTransient<MainMenu>();
        services.AddTransient<GeneratePassword>();
        services.AddTransient<HashPassword>();

        return services.BuildServiceProvider();
    }
}
