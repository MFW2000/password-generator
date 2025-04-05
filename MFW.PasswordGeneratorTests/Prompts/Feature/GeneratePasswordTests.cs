using MFW.PasswordGenerator.Prompts.Feature;
using MFW.PasswordGenerator.Records;
using MFW.PasswordGenerator.Services.Interfaces;
using Moq;
using TextCopy;

namespace MFW.PasswordGeneratorTests.Prompts.Feature;

[TestClass]
public class GeneratePasswordTests
{
    private Mock<IPasswordGeneratorService> _passwordGeneratorServiceMock = null!;
    private Mock<IClipboard> _clipboardMock = null!;

    private GeneratePassword _sut = null!;

    [TestInitialize]
    public void Initialize()
    {
        _passwordGeneratorServiceMock = new Mock<IPasswordGeneratorService>(MockBehavior.Strict);
        _clipboardMock = new Mock<IClipboard>(MockBehavior.Strict);

        _sut = new GeneratePassword(_passwordGeneratorServiceMock.Object, _clipboardMock.Object);
    }

    [TestMethod]
    public void DisplayPrompt_ShouldOutputExpectedText()
    {
        // Arrange
        const string password = "admin";

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
            .Returns(password)
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleOutput = new StringWriter();
        var consoleInput = new StringReader("");

        Console.SetOut(consoleOutput);
        Console.SetIn(consoleInput);

        // Act
        _sut.DisplayPrompt();

        // Assert
    }
}
