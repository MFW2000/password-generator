using JetBrains.Annotations;
using MFW.PasswordGenerator;
using MFW.PasswordGenerator.Infrastructure.Interfaces;
using MFW.PasswordGenerator.Presentation.Feature;
using MFW.PasswordGenerator.Records;
using MFW.PasswordGenerator.Services.Interfaces;
using Moq;
using TextCopy;

namespace MFW.PasswordGeneratorTests.Presentation.Feature;

[TestClass, UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class GenerateDefaultPasswordTests
{
    private Mock<IPasswordGeneratorService> _passwordGeneratorServiceMock = null!;
    private Mock<IClipboard> _clipboardMock = null!;
    private Mock<IConsoleLogger> _consoleLoggerMock = null!;

    private GenerateDefaultPassword _sut = null!;

    [TestInitialize]
    public void Initialize()
    {
        _passwordGeneratorServiceMock = new Mock<IPasswordGeneratorService>(MockBehavior.Strict);
        _clipboardMock = new Mock<IClipboard>(MockBehavior.Strict);
        _consoleLoggerMock = new Mock<IConsoleLogger>(MockBehavior.Strict);

        _sut = new GenerateDefaultPassword(
            _passwordGeneratorServiceMock.Object,
            _clipboardMock.Object,
            _consoleLoggerMock.Object);
    }

    [TestMethod]
    public void DisplayMainPrompt_ShouldOutputGenerationProcess()
    {
        // Arrange
        const string password = "admin";

        var input = string.Empty;

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
            .Returns(password)
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleInput = new StringReader(input);
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.Contains($"=== {CommonText.GenerateDefaultPasswordTitle} ===", output);
        Assert.Contains("Generate a new password with default secure settings.", output);
        Assert.Contains("Generating password...", output);
        Assert.Contains($"New password: {password}", output);
        Assert.Contains("Your new password was saved to your clipboard.", output);
        Assert.Contains(CommonText.TooltipContinue, output);

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_WithDefaults_ShouldGenerate()
    {
        // Arrange
        var input = string.Empty;

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.Is<PasswordGeneratorOptions>(y =>
                y.Length == Constants.PasswordLengthDefault
                && y.IncludeUppercase == Constants.UseUppercaseInPasswordDefault
                && y.IncludeLowercase == Constants.UseLowercaseInPasswordDefault
                && y.MinimumDigits == Constants.MinimumPasswordDigitsDefault
                && y.MinimumSpecialCharacters == Constants.MinimumSpecialPasswordCharactersDefault
                && y.AvoidAmbiguousCharacters == Constants.AvoidAmbiguousCharactersInPasswordDefault)))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleInput = new StringReader(input);
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PasswordGeneratorServiceThrowsException_ShouldDisplayError()
    {
        // Arrange
        var input = string.Empty;

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
            .Throws(new Exception());

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Never);

        _consoleLoggerMock
            .Setup(x => x.LogError(It.IsAny<string>(), It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleInput = new StringReader(input);
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.Contains("An error occurred while generating the password.", output);
        Assert.Contains("Press any key to continue.", output);

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
        _consoleLoggerMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_ClipboardServiceThrowsException_ShouldDisplayError()
    {
        // Arrange
        var input = string.Empty;

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Throws(new Exception());

        _consoleLoggerMock
            .Setup(x => x.LogError(It.IsAny<string>(), It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleInput = new StringReader(input);
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.Contains(
            "Your new password could not be saved to your clipboard, make sure you have the correct dependencies installed.",
            output);

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
        _consoleLoggerMock.Verify();
    }
}
