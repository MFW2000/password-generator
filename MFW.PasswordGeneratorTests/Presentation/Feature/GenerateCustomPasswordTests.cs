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
public class GenerateCustomPasswordTests
{
    private Mock<IPasswordGeneratorService> _passwordGeneratorServiceMock = null!;
    private Mock<IClipboard> _clipboardMock = null!;
    private Mock<IConsoleLogger> _consoleLoggerMock = null!;

    private GenerateCustomPassword _sut = null!;

    [TestInitialize]
    public void Initialize()
    {
        _passwordGeneratorServiceMock = new Mock<IPasswordGeneratorService>(MockBehavior.Strict);
        _clipboardMock = new Mock<IClipboard>(MockBehavior.Strict);
        _consoleLoggerMock = new Mock<IConsoleLogger>(MockBehavior.Strict);

        _sut = new GenerateCustomPassword(
            _passwordGeneratorServiceMock.Object,
            _clipboardMock.Object,
            _consoleLoggerMock.Object);
    }

    [TestMethod]
    public void DisplayMainPrompt_WithDefaultOptions_ShouldOutputGenerationProcess()
    {
        // Arrange
        const string password = "admin";
        const string input = "\n\n\n\n\n\n\n";

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

        Assert.Contains($"=== {CommonText.GenerateCustomPasswordTitle} ===", output);
        Assert.Contains("Generate a new password with the preferences of your choice.", output);
        Assert.Contains("--- Constraints ---", output);
        Assert.Contains("The password must comply with the following constraints:", output);
        Assert.Contains(
            $"- Length must be between {Constants.MinimumPasswordLength} and {Constants.MaximumPasswordLength} characters",
            output);
        Assert.Contains(
            "- The password must at least contain uppercases, lowercases, digits, or special characters",
            output);
        Assert.Contains(
            "- There may not be more digits and/or special characters than the length of the password",
            output);
        Assert.Contains("--- Preferences ---", output);
        Assert.Contains("Generating password...", output);
        Assert.Contains($"New password: {password}", output);
        Assert.Contains("Your new password was saved to your clipboard.", output);
        Assert.Contains(CommonText.TooltipContinue, output);

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptPasswordLength_WithValidInput_ShouldContinue()
    {
        // Arrange
        const int passwordLength = 12;

        var input = $"{passwordLength}\n";

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.Is<PasswordGeneratorOptions>(y =>
                y.Length == passwordLength)))
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
        var output = consoleOutput.ToString();

        Assert.Contains($"Enter the length of the password (default {Constants.PasswordLengthDefault}):", output);

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptPasswordLength_WithEmptyInput_ShouldContinueWithDefault()
    {
        // Arrange
        const string input = "\n";

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.Is<PasswordGeneratorOptions>(y =>
                y.Length == Constants.PasswordLengthDefault)))
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
        var output = consoleOutput.ToString();

        Assert.Contains($"Enter the length of the password (default {Constants.PasswordLengthDefault}):", output);

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptPasswordLength_WithLetterInput_ShouldDisplayError()
    {
        // Arrange
        const string input = "Help, I can't find the digits\n";

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        _consoleLoggerMock
            .Setup(x => x.LogInformation(It.IsAny<string>(), It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleInput = new StringReader(input);
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.Contains($"Enter the length of the password (default {Constants.PasswordLengthDefault}):", output);
        Assert.Contains(
            $"The password length must be a number between {Constants.MinimumPasswordLength} and {Constants.MaximumPasswordLength}.",
            output);

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
        _consoleLoggerMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptPasswordLength_WithSmallLength_ShouldDisplayError()
    {
        // Arrange
        const int smallPasswordLength = Constants.MinimumPasswordLength - 1;

        var input = $"{smallPasswordLength}\n";

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        _consoleLoggerMock
            .Setup(x => x.LogInformation(It.IsAny<string>(), It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleInput = new StringReader(input);
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.Contains($"Enter the length of the password (default {Constants.PasswordLengthDefault}):", output);
        Assert.Contains(
            $"The password length must be a number between {Constants.MinimumPasswordLength} and {Constants.MaximumPasswordLength}.",
            output);

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
        _consoleLoggerMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptPasswordLength_WithLargeLength_ShouldDisplayError()
    {
        // Arrange
        const int largePasswordLength = Constants.MaximumPasswordLength + 1;

        var input = $"{largePasswordLength}\n";

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        _consoleLoggerMock
            .Setup(x => x.LogInformation(It.IsAny<string>(), It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleInput = new StringReader(input);
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.Contains($"Enter the length of the password (default {Constants.PasswordLengthDefault}):", output);
        Assert.Contains(
            $"The password length must be a number between {Constants.MinimumPasswordLength} and {Constants.MaximumPasswordLength}.",
            output);

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
        _consoleLoggerMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptYesNo_OnUppercase_WithValidInput_ShouldContinue()
    {
        // Arrange
        const bool useUppercase = false;
        const string input = "\nno\n";

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.Is<PasswordGeneratorOptions>(y =>
                y.IncludeUppercase == useUppercase)))
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
        var output = consoleOutput.ToString();

        Assert.Contains("Include uppercase characters (YES/no):", output);

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptYesNo_OnUppercase_WithEmptyInput_ShouldContinueWithDefault()
    {
        // Arrange
        const string input = "\n\n";

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.Is<PasswordGeneratorOptions>(y =>
                y.IncludeUppercase == Constants.UseUppercaseInPasswordDefault)))
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
        var output = consoleOutput.ToString();

        Assert.Contains("Include uppercase characters (YES/no):", output);

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptYesNo_OnUppercase_WithInvalidInput_ShouldDisplayError()
    {
        // Arrange
        const string input = "\nbeep\n";

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
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
        var output = consoleOutput.ToString();

        Assert.Contains("Include uppercase characters (YES/no):", output);
        Assert.Contains("Please enter 'yes' (y) or 'no' (n).", output);

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptYesNo_OnLowercase_WithValidInput_ShouldContinue()
    {
        // Arrange
        const bool useLowercase = false;
        const string input = "\n\nno\n";

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.Is<PasswordGeneratorOptions>(y =>
                y.IncludeLowercase == useLowercase)))
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
        var output = consoleOutput.ToString();

        Assert.Contains("Include lowercase characters (YES/no):", output);

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptYesNo_OnLowercase_WithEmptyInput_ShouldContinueWithDefault()
    {
        // Arrange
        const string input = "\n\n\n";

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.Is<PasswordGeneratorOptions>(y =>
                y.IncludeLowercase == Constants.UseLowercaseInPasswordDefault)))
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
        var output = consoleOutput.ToString();

        Assert.Contains("Include lowercase characters (YES/no):", output);

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptYesNo_OnLowercase_WithInvalidInput_ShouldDisplayError()
    {
        // Arrange
        const string input = "\n\nbeep\n";

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
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
        var output = consoleOutput.ToString();

        Assert.Contains("Include lowercase characters (YES/no):", output);
        Assert.Contains("Please enter 'yes' (y) or 'no' (n).", output);

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptMinimumDigits_WithValidInput_ShouldContinue()
    {
        // Arrange
        const int minimumDigits = 2;

        var input = $"\n\n\n{minimumDigits}\n";

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.Is<PasswordGeneratorOptions>(y =>
                y.MinimumDigits == minimumDigits)))
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
        var output = consoleOutput.ToString();

        Assert.Contains(
            $"Enter the minimum number of digits to be included (default {Constants.MinimumPasswordDigitsDefault}):",
            output);

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptMinimumDigits_WithEmptyInput_ShouldContinueWithDefault()
    {
        // Arrange
        const string input = "\n\n\n\n";

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.Is<PasswordGeneratorOptions>(y =>
                y.MinimumDigits == Constants.MinimumPasswordDigitsDefault)))
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
        var output = consoleOutput.ToString();

        Assert.Contains(
            $"Enter the minimum number of digits to be included (default {Constants.MinimumPasswordDigitsDefault}):",
            output);

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptMinimumDigits_WithLetterInput_ShouldDisplayError()
    {
        // Arrange
        const string input = "\n\n\nThe numbers, Mason!\n";

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        _consoleLoggerMock
            .Setup(x => x.LogInformation(It.IsAny<string>(), It.IsAny<string>()))
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
            $"Enter the minimum number of digits to be included (default {Constants.MinimumPasswordDigitsDefault}):",
            output);
        Assert.Contains(
            "The minimum number of digits must be a non-negative number and cannot exceed the password length.",
            output);

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
        _consoleLoggerMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptMinimumDigits_WithNegativeInput_ShouldDisplayError()
    {
        // Arrange
        const string input = "\n\n\n-1\n";

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        _consoleLoggerMock
            .Setup(x => x.LogInformation(It.IsAny<string>(), It.IsAny<string>()))
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
            $"Enter the minimum number of digits to be included (default {Constants.MinimumPasswordDigitsDefault}):",
            output);
        Assert.Contains(
            "The minimum number of digits must be a non-negative number and cannot exceed the password length.",
            output);

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
        _consoleLoggerMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptMinimumDigits_WithExceedingCount_ShouldDisplayError()
    {
        // Arrange
        const int passwordLength = 10;
        const int minimumDigits = 11;

        var input = $"{passwordLength}\n\n\n{minimumDigits}\n";

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        _consoleLoggerMock
            .Setup(x => x.LogInformation(It.IsAny<string>(), It.IsAny<string>()))
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
            $"Enter the minimum number of digits to be included (default {Constants.MinimumPasswordDigitsDefault}):",
            output);
        Assert.Contains(
            "The minimum number of digits must be a non-negative number and cannot exceed the password length.",
            output);

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
        _consoleLoggerMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptMinimumSpecialCharacters_WithValidInput_ShouldContinue()
    {
        // Arrange
        const int minimumSpecialCharacters = 2;

        var input = $"\n\n\n\n{minimumSpecialCharacters}\n";

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.Is<PasswordGeneratorOptions>(y =>
                y.MinimumSpecialCharacters == minimumSpecialCharacters)))
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
        var output = consoleOutput.ToString();

        Assert.Contains(
            $"Enter the minimum number of special characters to be included (default {Constants.MinimumSpecialPasswordCharactersDefault}):",
            output);

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptMinimumSpecialCharacters_WithEmptyInput_ShouldContinueWithDefault()
    {
        // Arrange
        const string input = "\n\n\n\n\n";

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.Is<PasswordGeneratorOptions>(y =>
                y.MinimumSpecialCharacters == Constants.MinimumSpecialPasswordCharactersDefault)))
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
        var output = consoleOutput.ToString();

        Assert.Contains(
            $"Enter the minimum number of special characters to be included (default {Constants.MinimumSpecialPasswordCharactersDefault}):",
            output);

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptMinimumSpecialCharacters_WithNoRoom_ShouldDisplayError()
    {
        // Arrange
        const int passwordLength = 10;
        const int minimumDigits = 10;
        const int minimumSpecialCharacters = 2;

        var input = $"{passwordLength}\n\n\n{minimumDigits}\n{minimumSpecialCharacters}\n";

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
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
        var output = consoleOutput.ToString();

        Assert.Contains(
            "Special character input skipped, there is no room left for additional special characters.",
            output);

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptMinimumSpecialCharacters_WithLetterInput_ShouldDisplayError()
    {
        // Arrange
        const string input = "\n\n\n\nhaha money printer go brr\n";

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        _consoleLoggerMock
            .Setup(x => x.LogInformation(It.IsAny<string>(), It.IsAny<string>()))
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
            $"Enter the minimum number of special characters to be included (default {Constants.MinimumSpecialPasswordCharactersDefault}):",
            output);
        Assert.Contains(
            """
            The minimum number of special characters must be a non-negative number and the combined total of special characters and
            digits cannot exceed the password's length.
            """,
            output);

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
        _consoleLoggerMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptMinimumSpecialCharacters_WithNegativeInput_ShouldDisplayError()
    {
        // Arrange
        const string input = "\n\n\n\n-1\n";

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        _consoleLoggerMock
            .Setup(x => x.LogInformation(It.IsAny<string>(), It.IsAny<string>()))
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
            $"Enter the minimum number of special characters to be included (default {Constants.MinimumSpecialPasswordCharactersDefault}):",
            output);
        Assert.Contains(
            """
            The minimum number of special characters must be a non-negative number and the combined total of special characters and
            digits cannot exceed the password's length.
            """,
            output);

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
        _consoleLoggerMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptMinimumSpecialCharacters_WithExceedingCount_ShouldDisplayError()
    {
        // Arrange
        const int passwordLength = 10;
        const int minimumDigits = 5;
        const int minimumSpecialCharacters = 6;

        var input = $"{passwordLength}\n\n\n{minimumDigits}\n{minimumSpecialCharacters}\n";

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        _consoleLoggerMock
            .Setup(x => x.LogInformation(It.IsAny<string>(), It.IsAny<string>()))
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
            $"Enter the minimum number of special characters to be included (default {Constants.MinimumSpecialPasswordCharactersDefault}):",
            output);
        Assert.Contains(
            """
            The minimum number of special characters must be a non-negative number and the combined total of special characters and
            digits cannot exceed the password's length.
            """,
            output);

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
        _consoleLoggerMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptYesNo_OnAmbiguous_WithValidInput_ShouldContinue()
    {
        // Arrange
        const bool useAmbiguous = false;

        var input = $"\n\n\n\n\n{useAmbiguous}\n";

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.Is<PasswordGeneratorOptions>(y =>
                y.AvoidAmbiguousCharacters == useAmbiguous)))
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
        var output = consoleOutput.ToString();

        Assert.Contains("Avoid ambiguous characters (yes/NO):", output);

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptYesNo_OnAmbiguous_WithEmptyInput_ShouldContinueWithDefault()
    {
        // Arrange
        const string input = "\n\n\n\n\n\n";

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.Is<PasswordGeneratorOptions>(y =>
                y.AvoidAmbiguousCharacters == Constants.AvoidAmbiguousCharactersInPasswordDefault)))
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
        var output = consoleOutput.ToString();

        Assert.Contains("Avoid ambiguous characters (yes/NO):", output);

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptYesNo_OnAmbiguous_WithInvalidInput_ShouldDisplayError()
    {
        // Arrange
        const string input = "\n\n\n\n\nmeow\n";

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
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
        var output = consoleOutput.ToString();

        Assert.Contains("Avoid ambiguous characters (yes/NO):", output);
        Assert.Contains("Please enter 'yes' (y) or 'no' (n).", output);

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_WithInvalidConfiguration_ShouldDisplayError()
    {
        // Arrange
        const string input = "\nno\nno\n0\n0\n\n";

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Never);

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

        Assert.Contains(
            "Password options must include at least one of: uppercase letters, lowercase letters, digits, or special characters.",
            output);

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PasswordGeneratorServiceThrowsException_ShouldDisplayError()
    {
        // Arrange
        const string input = "\n\n\n\n\n\n\n";

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
        const string input = "\n\n\n\n\n\n\n";

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
