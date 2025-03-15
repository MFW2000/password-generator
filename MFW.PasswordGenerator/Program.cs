using MFW.PasswordGenerator.Core;
using MFW.PasswordGenerator.Core.Interfaces;
using MFW.PasswordGenerator.Factories;
using MFW.PasswordGenerator.Factories.Interfaces;
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
        var runner = serviceProvider.GetRequiredService<IPromptRunner>();

        runner.Run();
    }

    private static ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        // Register services.
        services.AddSingleton<IPromptRunner, PromptRunner>();
        services.AddSingleton<IAssemblyVersionProvider, AssemblyVersionProvider>();
        services.AddTransient<IPromptFactory, PromptFactory>();
        services.AddTransient<MainMenu>();
        services.AddTransient<GeneratePassword>();
        services.AddTransient<HashPassword>();

        return services.BuildServiceProvider();
    }
}
