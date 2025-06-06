﻿using MFW.PasswordGenerator;
using MFW.PasswordGenerator.Enumerations;
using MFW.PasswordGenerator.Factories.Interfaces;
using MFW.PasswordGenerator.Infrastructure.Interfaces;
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

    private PromptRunner _sut = null!;

    [TestInitialize]
    public void Setup()
    {
        _assemblyVersionProviderMock = new Mock<IAssemblyVersionProvider>();
        _promptFactoryMock = new Mock<IPromptFactory>(MockBehavior.Strict);
        _consoleClearMock = new Mock<IConsoleClear>(MockBehavior.Strict);
        _mainMenuMock = new Mock<MainMenu>(MockBehavior.Strict, _assemblyVersionProviderMock.Object);

        _sut = new PromptRunner(_promptFactoryMock.Object, _consoleClearMock.Object);
    }

    [TestMethod]
    public void Run_NavigatesToExit()
    {
        // Arrange
        _mainMenuMock
            .Setup(x => x.DisplayMainPrompt())
            .Returns((PromptType?)null)
            .Verifiable(Times.Once);

        _promptFactoryMock
            .Setup(x => x.CreatePrompt<MainMenu>())
            .Returns(_mainMenuMock.Object)
            .Verifiable(Times.Once);

        _consoleClearMock
            .Setup(x => x.Clear())
            .Verifiable(Times.Once);

        var consoleOutput = new StringWriter();

        Console.SetOut(consoleOutput);

        // Act
        _sut.Run();

        // Assert
        _mainMenuMock.Verify();
        _promptFactoryMock.Verify();
    }
}
