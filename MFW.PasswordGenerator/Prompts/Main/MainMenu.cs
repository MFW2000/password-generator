using MFW.PasswordGenerator.Prompts.Feature;

namespace MFW.PasswordGenerator.Prompts.Main;

/// <summary>
/// Responsible for guiding the user to all application features.
/// </summary>
public class MainMenu : Prompt
{
    public override void DisplayPrompt()
    {
        Console.WriteLine($"=== {Constants.AppTitle} v{Program.GetApplicationVersion()} ===");
        Console.WriteLine(Constants.AppSubTitle);
        Console.WriteLine();
        Console.WriteLine(Constants.TooltipOption);
        Console.WriteLine("1. Generate password");
        Console.WriteLine("2. Hash password");
        Console.WriteLine("3. Exit");
    }

    public override Prompt? HandlePrompt()
    {
        while (true)
        {
            Console.Write(Constants.InputPrompt);

            var input = Console.ReadLine() ?? string.Empty;

            switch (input.ToLower())
            {
                case "1":
                    return new GeneratePassword();
                case "2":
                    return new HashPassword();
                case "3":
                    return null;
                default:
                    Console.WriteLine(Constants.InputError);
                    break;
            }
        }
    }
}
