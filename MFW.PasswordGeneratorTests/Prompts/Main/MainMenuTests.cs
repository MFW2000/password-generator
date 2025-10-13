using JetBrains.Annotations;
using MFW.PasswordGenerator;
using MFW.PasswordGenerator.Enumerations;
using MFW.PasswordGenerator.Prompts.Main;
using MFW.PasswordGenerator.Providers.Interfaces;
using Moq;

namespace MFW.PasswordGeneratorTests.Prompts.Main;

[TestClass, UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class MainMenuTests
{
    private Mock<IAssemblyVersionProvider> _assemblyVersionProviderMock = null!;

    private MainMenu _sut = null!;

    [TestInitialize]
    public void Setup()
    {
        _assemblyVersionProviderMock = new Mock<IAssemblyVersionProvider>(MockBehavior.Strict);

        _sut = new MainMenu(_assemblyVersionProviderMock.Object);
    }

    [TestMethod]
    public void DisplayMainPrompt_ShouldOutputMenuAndVersion()
    {
        // Arrange
        const string input = "3\n";

        var version = new Version(1, 2, 3);

        _assemblyVersionProviderMock
            .Setup(x => x.GetVersion())
            .Returns(version)
            .Verifiable(Times.Once);

        var consoleInput = new StringReader(input);
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.Contains($"=== {CommonText.AppTitle} v{version.ToString(3)}", output);
        Assert.Contains(CommonText.AppSubTitle, output);
        Assert.Contains("--- Main Menu ---", output);
        Assert.Contains($"1. {CommonText.GenerateDefaultPasswordTitle}", output);
        Assert.Contains($"2. {CommonText.GenerateCustomPasswordTitle}", output);
        Assert.Contains("3. Exit", output);
        Assert.Contains(CommonText.TooltipOption, output);

        _assemblyVersionProviderMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_WithValidInput_ShouldReturnCorrectPrompt()
    {
        // Arrange
        const string input = "2\n";

        var version = new Version(1, 2, 3);

        _assemblyVersionProviderMock
            .Setup(x => x.GetVersion())
            .Returns(version)
            .Verifiable(Times.Once);

        var consoleInput = new StringReader(input);

        Console.SetIn(consoleInput);

        // Act
        var result = _sut.DisplayMainPrompt();

        // Assert
        Assert.AreEqual(PromptType.GenerateCustomPassword, result);

        _assemblyVersionProviderMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_WithExitInput_ShouldReturnNull()
    {
        // Arrange
        const string input = "3\n";

        var version = new Version(1, 2, 3);

        _assemblyVersionProviderMock
            .Setup(x => x.GetVersion())
            .Returns(version)
            .Verifiable(Times.Once);

        var consoleInput = new StringReader(input);

        Console.SetIn(consoleInput);

        // Act
        var result = _sut.DisplayMainPrompt();

        // Assert
        Assert.IsNull(result);

        _assemblyVersionProviderMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_WithInvalidInput_ShouldOutputError()
    {
        // Arrange
        const string input = "invalid\n1\n";

        var version = new Version(1, 2, 3);

        _assemblyVersionProviderMock
            .Setup(x => x.GetVersion())
            .Returns(version)
            .Verifiable(Times.Once);

        var consoleInput = new StringReader(input);
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        var result = _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.Contains("Please select a valid menu option.", output);
        Assert.IsNotNull(result);

        _assemblyVersionProviderMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_WithEmptyInput_ShouldOutputError()
    {
        // Arrange
        const string input = "\n1\n";

        var version = new Version(1, 2, 3);

        _assemblyVersionProviderMock
            .Setup(x => x.GetVersion())
            .Returns(version)
            .Verifiable(Times.Once);

        var consoleInput = new StringReader(input);
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        var result = _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.Contains("Please select a valid menu option.", output);
        Assert.IsNotNull(result);

        _assemblyVersionProviderMock.Verify();
    }
}
