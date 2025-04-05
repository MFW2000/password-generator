using MFW.PasswordGenerator.Prompts.Feature;
using MFW.PasswordGenerator.Records;
using MFW.PasswordGenerator.Services.Interfaces;
using Moq;

namespace MFW.PasswordGeneratorTests.Prompts.Feature;

[TestClass]
public class GeneratePasswordTests
{
    private Mock<IPasswordGeneratorService> _passwordGeneratorServiceMock = null!;

    private GeneratePassword _sut = null!;

    [TestInitialize]
    public void Initialize()
    {
        _passwordGeneratorServiceMock = new Mock<IPasswordGeneratorService>(MockBehavior.Strict);

        _sut = new GeneratePassword(_passwordGeneratorServiceMock.Object);
    }

    // TODO: Finish this test later.

    [TestMethod]
    public void DisplayPrompt_ShouldOutputExpectedText()
    {
        // Arrange
        const string password = "admin";

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
            .Returns(password)
            .Verifiable(Times.Once);

        // Act
        _sut.DisplayPrompt();

        // Assert
    }
}
