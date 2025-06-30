using MFW.PasswordGenerator;
using MFW.PasswordGenerator.Enumerations;
using MFW.PasswordGenerator.Prompts.Main;
using MFW.PasswordGenerator.Providers.Interfaces;
using Moq;

namespace MFW.PasswordGeneratorTests.Prompts.Main;

[TestClass]
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

        Assert.IsTrue(output.Contains($"=== {CommonText.AppTitle} v{version.ToString(3)}"));
        Assert.IsTrue(output.Contains(CommonText.AppSubTitle));
        Assert.IsTrue(output.Contains("--- Main Menu ---"));
        Assert.IsTrue(output.Contains($"1. {CommonText.GenerateDefaultPasswordTitle}"));
        Assert.IsTrue(output.Contains($"2. {CommonText.GenerateCustomPasswordTitle}"));
        Assert.IsTrue(output.Contains($"3. {CommonText.HashPasswordTitle}"));
        Assert.IsTrue(output.Contains("4. Exit"));
        Assert.IsTrue(output.Contains(CommonText.TooltipOption));

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
        const string input = "4\n";

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

        Assert.IsTrue(output.Contains("Please select a valid menu option number."));
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

        Assert.IsTrue(output.Contains("Please select a valid menu option number."));
        Assert.IsNotNull(result);

        _assemblyVersionProviderMock.Verify();
    }
}
