using MFW.PasswordGenerator;
using MFW.PasswordGenerator.Prompts.Feature;
using MFW.PasswordGenerator.Records;
using MFW.PasswordGenerator.Services.Interfaces;
using Moq;
using TextCopy;

namespace MFW.PasswordGeneratorTests.Prompts.Feature;

[TestClass]
public class GenerateDefaultPasswordTests
{
    private Mock<IPasswordGeneratorService> _passwordGeneratorServiceMock = null!;
    private Mock<IClipboard> _clipboardMock = null!;

    private GenerateDefaultPassword _sut = null!;

    [TestInitialize]
    public void Initialize()
    {
        _passwordGeneratorServiceMock = new Mock<IPasswordGeneratorService>(MockBehavior.Strict);
        _clipboardMock = new Mock<IClipboard>(MockBehavior.Strict);

        _sut = new GenerateDefaultPassword(_passwordGeneratorServiceMock.Object, _clipboardMock.Object);
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

        Assert.IsTrue(output.Contains($"=== {CommonText.GenerateDefaultPasswordTitle} ==="));
        Assert.IsTrue(output.Contains("Generate a new password with default secure settings."));
        Assert.IsTrue(output.Contains("Generating password..."));
        Assert.IsTrue(output.Contains($"New password: {password}"));
        Assert.IsTrue(output.Contains("The password was saved to your clipboard."));
        Assert.IsTrue(output.Contains(CommonText.TooltipContinue));

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

        var consoleInput = new StringReader(input);
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains("An error occurred while generating the password."));
        Assert.IsTrue(output.Contains("Press any key to continue."));

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
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

        var consoleInput = new StringReader(input);
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains(
            "The password could not be saved to your clipboard, make sure you have the correct " +
            "dependencies installed."));

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }
}
