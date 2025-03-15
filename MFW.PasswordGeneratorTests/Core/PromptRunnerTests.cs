using MFW.PasswordGenerator.Core;
using MFW.PasswordGenerator.Enumerations;
using MFW.PasswordGenerator.Factories.Interfaces;
using MFW.PasswordGenerator.Prompts.Feature;
using MFW.PasswordGenerator.Prompts.Main;
using MFW.PasswordGenerator.Providers.Interfaces;
using Moq;

namespace MFW.PasswordGeneratorTests.Core;

[TestClass]
public class PromptRunnerTests
{
    /*
    private Mock<IAssemblyVersionProvider> _assemblyVersionProviderMock = null!;
    private Mock<IPromptFactory> _promptFactoryMock = null!;
    private Mock<MainMenu> _mainMenuMock = null!;
    private Mock<GeneratePassword> _generatePasswordMock  = null!;
    private PromptRunner _sut = null!;

    [TestInitialize]
    public void Setup()
    {
        _assemblyVersionProviderMock = new Mock<IAssemblyVersionProvider>();
        _promptFactoryMock = new Mock<IPromptFactory>(MockBehavior.Strict);
        _mainMenuMock = new Mock<MainMenu>(MockBehavior.Strict, _assemblyVersionProviderMock.Object);
        _generatePasswordMock = new Mock<GeneratePassword>(MockBehavior.Strict);
        _sut = new PromptRunner(_promptFactoryMock.Object);
    }

    [TestMethod]
    public void Run_NavigatesFromMainMenuToGeneratePassword()
    {
        // Arrange
        const string mainMenuString = "MainMenu";
        const string generatePasswordString = "GeneratePassword";

        _mainMenuMock
            .Setup(x => x.DisplayPrompt())
            .Callback(() => Console.WriteLine(mainMenuString))
            .Verifiable(Times.Once);
        _mainMenuMock
            .Setup(x => x.HandlePrompt())
            .Returns(PromptType.GeneratePassword)
            .Verifiable(Times.Once);

        _generatePasswordMock
            .Setup(x => x.DisplayPrompt())
            .Callback(() => Console.WriteLine(generatePasswordString))
            .Verifiable(Times.Once);
        _generatePasswordMock
            .Setup(x => x.HandlePrompt())
            .Returns((PromptType?)null)
            .Verifiable(Times.Once);

        _promptFactoryMock
            .Setup(x => x.CreatePrompt<MainMenu>())
            .Returns(_mainMenuMock.Object)
            .Verifiable(Times.Once);
        _promptFactoryMock
            .Setup(x => x.CreatePrompt<GeneratePassword>())
            .Returns(_generatePasswordMock.Object)
            .Verifiable(Times.Once);

        var consoleOutput = new StringWriter();

        Console.SetOut(consoleOutput);

        // Act
        _sut.Run();

        // Assert
        var output = consoleOutput.ToString();

        Assert.Contains(mainMenuString, output);
        Assert.Contains(generatePasswordString, output);

        _mainMenuMock.Verify();
        _generatePasswordMock.Verify();
        _promptFactoryMock.Verify();
    }
    */
}
