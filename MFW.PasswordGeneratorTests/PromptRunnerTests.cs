using MFW.PasswordGenerator;
using MFW.PasswordGenerator.Enumerations;
using MFW.PasswordGenerator.Factories.Interfaces;
using MFW.PasswordGenerator.Infrastructure.Interfaces;
using MFW.PasswordGenerator.Prompts.Feature;
using MFW.PasswordGenerator.Prompts.Main;
using MFW.PasswordGenerator.Providers.Interfaces;
using Moq;

namespace MFW.PasswordGeneratorTests;

[TestClass]
public class PromptRunnerTests
{
    private Mock<IAssemblyVersionProvider> _assemblyVersionProviderMock = null!;
    private Mock<IPromptFactory> _promptFactoryMock = null!;
    private Mock<IConsoleClear> _consoleClearMock = null!;
    private Mock<MainMenu> _mainMenuMock = null!;
    private Mock<GeneratePassword> _generatePasswordMock  = null!;
    private PromptRunner _sut = null!;

    [TestInitialize]
    public void Setup()
    {
        _assemblyVersionProviderMock = new Mock<IAssemblyVersionProvider>();
        _promptFactoryMock = new Mock<IPromptFactory>(MockBehavior.Strict);
        _consoleClearMock = new Mock<IConsoleClear>(MockBehavior.Strict);
        _mainMenuMock = new Mock<MainMenu>(MockBehavior.Strict, _assemblyVersionProviderMock.Object);
        _generatePasswordMock = new Mock<GeneratePassword>(MockBehavior.Strict);
        _sut = new PromptRunner(_promptFactoryMock.Object, _consoleClearMock.Object);
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

        _consoleClearMock
            .Setup(x => x.Clear())
            .Verifiable(Times.Exactly(2));

        var consoleOutput = new StringWriter();

        Console.SetOut(consoleOutput);

        // Act
        _sut.Run();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains(mainMenuString));
        Assert.IsTrue(output.Contains(generatePasswordString));

        _mainMenuMock.Verify();
        _generatePasswordMock.Verify();
        _promptFactoryMock.Verify();
    }
}
