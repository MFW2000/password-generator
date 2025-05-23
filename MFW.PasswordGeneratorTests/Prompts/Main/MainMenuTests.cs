﻿using MFW.PasswordGenerator.Enumerations;
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
        var version = new Version(1, 2, 3);

        _assemblyVersionProviderMock
            .Setup(x => x.GetVersion())
            .Returns(version)
            .Verifiable(Times.Once);

        var consoleOutput = new StringWriter();
        var consoleInput = new StringReader("3\n");

        Console.SetOut(consoleOutput);
        Console.SetIn(consoleInput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains($"=== Password Generator v{version.ToString(3)}"));
        Assert.IsTrue(output.Contains("Generate and/or hash passwords."));
        Assert.IsTrue(output.Contains("Select an option:"));
        Assert.IsTrue(output.Contains("1. Generate password"));
        Assert.IsTrue(output.Contains("2. Hash password"));
        Assert.IsTrue(output.Contains("3. Exit"));

        _assemblyVersionProviderMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_ShouldReturnCorrectPrompt_ForValidInput()
    {
        // Arrange
        var version = new Version(1, 2, 3);

        _assemblyVersionProviderMock
            .Setup(x => x.GetVersion())
            .Returns(version)
            .Verifiable(Times.Once);

        var consoleInput = new StringReader("1\n");

        Console.SetIn(consoleInput);

        // Act
        var result = _sut.DisplayMainPrompt();

        // Assert
        Assert.AreEqual(PromptType.GeneratePassword, result);

        _assemblyVersionProviderMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_ShouldReturnNull_ForExitInput()
    {
        // Arrange
        var version = new Version(1, 2, 3);

        _assemblyVersionProviderMock
            .Setup(x => x.GetVersion())
            .Returns(version)
            .Verifiable(Times.Once);

        var consoleInput = new StringReader("3\n");

        Console.SetIn(consoleInput);

        // Act
        var result = _sut.DisplayMainPrompt();

        // Assert
        Assert.IsNull(result);

        _assemblyVersionProviderMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_ShouldOutputError_ForInvalidInput()
    {
        // Arrange
        var version = new Version(1, 2, 3);

        _assemblyVersionProviderMock
            .Setup(x => x.GetVersion())
            .Returns(version)
            .Verifiable(Times.Once);

        var consoleInput = new StringReader("invalid\n1\n");

        Console.SetIn(consoleInput);

        var consoleOutput = new StringWriter();

        Console.SetOut(consoleOutput);

        // Act
        var result = _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains("Invalid option, try again."));
        Assert.IsNotNull(result);

        _assemblyVersionProviderMock.Verify();
    }
}
