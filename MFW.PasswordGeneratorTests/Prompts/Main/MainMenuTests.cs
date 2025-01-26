using MFW.PasswordGenerator.Prompts.Feature;
using MFW.PasswordGenerator.Prompts.Main;

namespace MFW.PasswordGeneratorTests.Prompts.Main;

[TestClass]
public class MainMenuTests
{
    private MainMenu _mainMenu = null!;

    [TestInitialize]
    public void Setup()
    {
        _mainMenu = new MainMenu();
    }

    [TestMethod]
    public void DisplayPrompt_ShouldOutputExpectedText()
    {
        // Arrange
        var consoleOutput = new StringWriter();

        Console.SetOut(consoleOutput);

        // Act
        _mainMenu.DisplayPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains("=== Password Generator v"));
        Assert.IsTrue(output.Contains("Generate and/or hash passwords."));
        Assert.IsTrue(output.Contains("Select an option:"));
        Assert.IsTrue(output.Contains("1. Generate password"));
        Assert.IsTrue(output.Contains("2. Hash password"));
        Assert.IsTrue(output.Contains("3. Exit"));
    }

    [TestMethod]
    public void HandlePrompt_ShouldReturnCorrectPrompt_ForValidInput()
    {
        // Arrange
        var consoleInput = new StringReader("1\n");

        Console.SetIn(consoleInput);

        // Act
        var result = _mainMenu.HandlePrompt();

        // Assert
        Assert.IsInstanceOfType(result, typeof(GeneratePassword));
    }

    [TestMethod]
    public void HandlePrompt_ShouldReturnNull_ForExitInput()
    {
        // Arrange
        var consoleInput = new StringReader("3\n");

        Console.SetIn(consoleInput);

        // Act
        var result = _mainMenu.HandlePrompt();

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void HandlePrompt_ShouldOutputError_ForInvalidInput()
    {
        // Arrange
        var consoleInput = new StringReader("invalid\n1\n");

        Console.SetIn(consoleInput);

        var consoleOutput = new StringWriter();

        Console.SetOut(consoleOutput);

        // Act
        var result = _mainMenu.HandlePrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains("Invalid option, try again."));
        Assert.IsNotNull(result);
    }
}
