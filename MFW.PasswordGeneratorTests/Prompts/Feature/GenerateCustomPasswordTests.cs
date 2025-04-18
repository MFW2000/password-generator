using MFW.PasswordGenerator;
using MFW.PasswordGenerator.Prompts.Feature;
using MFW.PasswordGenerator.Records;
using MFW.PasswordGenerator.Services.Interfaces;
using Moq;
using TextCopy;

namespace MFW.PasswordGeneratorTests.Prompts.Feature;

[TestClass]
public class GenerateCustomPasswordTests
{
    private Mock<IPasswordGeneratorService> _passwordGeneratorServiceMock = null!;
    private Mock<IClipboard> _clipboardMock = null!;

    private GenerateCustomPassword _sut = null!;

    [TestInitialize]
    public void Initialize()
    {
        _passwordGeneratorServiceMock = new Mock<IPasswordGeneratorService>(MockBehavior.Strict);
        _clipboardMock = new Mock<IClipboard>(MockBehavior.Strict);

        _sut = new GenerateCustomPassword(_passwordGeneratorServiceMock.Object, _clipboardMock.Object);
    }

    [TestMethod]
    public void DisplayMainPrompt_WithDefaultOptions_ShouldOutputGenerationProcess()
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

        var consoleInput = new StringReader("\n\n\n\n\n\n\n");
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains($"=== {CommonText.GenerateCustomPasswordTitle} ==="));
        Assert.IsTrue(output.Contains("Generate a new password with the preferences of your choice."));
        Assert.IsTrue(output.Contains("--- Constraints ---"));
        Assert.IsTrue(output.Contains("The password must comply with the following constraints:"));
        Assert.IsTrue(output.Contains(
            $"- Length must be between {Constants.MinimumPasswordLength} and " +
            $"{Constants.MaximumPasswordLength} characters"));
        Assert.IsTrue(output.Contains(
            "- The password must at least contain uppercases, lowercases, digits, or special characters"));
        Assert.IsTrue(output.Contains(
            "- There may not be more digits and/or special characters than the length of the password"));
        Assert.IsTrue(output.Contains("--- Preferences ---"));
        Assert.IsTrue(output.Contains("Generating password..."));
        Assert.IsTrue(output.Contains($"New password: {password}"));
        Assert.IsTrue(output.Contains("The password was saved to your clipboard."));
        Assert.IsTrue(output.Contains(CommonText.TooltipContinue));

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptPasswordLength_WithValidInput_ShouldContinue()
    {
        // Arrange
        const int passwordLength = 12;

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.Is<PasswordGeneratorOptions>(y =>
                y.Length == passwordLength)))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleInput = new StringReader($"{passwordLength}\n");
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains(
            $"Enter the length of the password (default {Constants.PasswordLengthDefault}):"));

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptPasswordLength_WithEmptyInput_ShouldContinueWithDefault()
    {
        // Arrange
        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.Is<PasswordGeneratorOptions>(y =>
                y.Length == Constants.PasswordLengthDefault)))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleInput = new StringReader("\n");
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains(
            $"Enter the length of the password (default {Constants.PasswordLengthDefault}):"));

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptPasswordLength_WithLetterInput_ShouldDisplayError()
    {
        // Arrange
        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleInput = new StringReader("Help, I can't find the digits\n");
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains(
            $"Enter the length of the password (default {Constants.PasswordLengthDefault}):"));
        Assert.IsTrue(output.Contains(
            $"The password length must be a number between {Constants.MinimumPasswordLength} " +
            $"and {Constants.MaximumPasswordLength}."));

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptPasswordLength_WithSmallLength_ShouldDisplayError()
    {
        // Arrange
        const int smallPasswordLength = Constants.MinimumPasswordLength - 1;

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleInput = new StringReader($"{smallPasswordLength}\n");
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains(
            $"Enter the length of the password (default {Constants.PasswordLengthDefault}):"));
        Assert.IsTrue(output.Contains(
            $"The password length must be a number between {Constants.MinimumPasswordLength} " +
            $"and {Constants.MaximumPasswordLength}."));

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptPasswordLength_WithLargeLength_ShouldDisplayError()
    {
        // Arrange
        const int largePasswordLength = Constants.MaximumPasswordLength + 1;

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleInput = new StringReader($"{largePasswordLength}\n");
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains(
            $"Enter the length of the password (default {Constants.PasswordLengthDefault}):"));
        Assert.IsTrue(output.Contains(
            $"The password length must be a number between {Constants.MinimumPasswordLength} " +
            $"and {Constants.MaximumPasswordLength}."));

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptYesNo_OnUppercase_WithValidInput_ShouldContinue()
    {
        // Arrange
        const bool useUppercase = false;

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.Is<PasswordGeneratorOptions>(y =>
                y.IncludeUppercase == useUppercase)))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleInput = new StringReader("\nno\n");
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains("Include uppercase characters (YES/no):"));

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptYesNo_OnUppercase_WithEmptyInput_ShouldContinueWithDefault()
    {
        // Arrange
        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.Is<PasswordGeneratorOptions>(y =>
                y.IncludeUppercase == Constants.UseUppercaseInPasswordDefault)))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleInput = new StringReader("\n\n");
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains("Include uppercase characters (YES/no):"));

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptYesNo_OnUppercase_WithInvalidInput_ShouldDisplayError()
    {
        // Arrange
        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleInput = new StringReader("\nbeep\n");
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains("Include uppercase characters (YES/no):"));
        Assert.IsTrue(output.Contains("Please enter 'yes' (y) or 'no' (n)."));

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptYesNo_OnLowercase_WithValidInput_ShouldContinue()
    {
        // Arrange
        const bool useLowercase = false;

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.Is<PasswordGeneratorOptions>(y =>
                y.IncludeLowercase == useLowercase)))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleInput = new StringReader("\n\nno\n");
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains("Include lowercase characters (YES/no):"));

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptYesNo_OnLowercase_WithEmptyInput_ShouldContinueWithDefault()
    {
        // Arrange
        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.Is<PasswordGeneratorOptions>(y =>
                y.IncludeLowercase == Constants.UseLowercaseInPasswordDefault)))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleInput = new StringReader("\n\n\n");
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains("Include lowercase characters (YES/no):"));

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptYesNo_OnLowercase_WithInvalidInput_ShouldDisplayError()
    {
        // Arrange
        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleInput = new StringReader("\n\nbeep\n");
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains("Include lowercase characters (YES/no):"));
        Assert.IsTrue(output.Contains("Please enter 'yes' (y) or 'no' (n)."));

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptMinimumDigits_WithValidInput_ShouldContinue()
    {
        // Arrange
        const int minimumDigits = 2;

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.Is<PasswordGeneratorOptions>(y =>
                y.MinimumDigits == minimumDigits)))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleInput = new StringReader($"\n\n\n{minimumDigits}\n");
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains(
            $"Enter the minimum number of digits to be included (default {Constants.MinimumPasswordDigitsDefault}):"));

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptMinimumDigits_WithEmptyInput_ShouldContinueWithDefault()
    {
        // Arrange
        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.Is<PasswordGeneratorOptions>(y =>
                y.MinimumDigits == Constants.MinimumPasswordDigitsDefault)))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleInput = new StringReader("\n\n\n\n");
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains(
            $"Enter the minimum number of digits to be included (default {Constants.MinimumPasswordDigitsDefault}):"));

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptMinimumDigits_WithLetterInput_ShouldDisplayError()
    {
        // Arrange
        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleInput = new StringReader("\n\n\nThe numbers, Mason!\n");
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains(
            $"Enter the minimum number of digits to be included (default {Constants.MinimumPasswordDigitsDefault}):"));
        Assert.IsTrue(output.Contains(
            "The minimum number of digits must be a non-negative number and cannot exceed the password length."));

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptMinimumDigits_WithNegativeInput_ShouldDisplayError()
    {
        // Arrange
        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleInput = new StringReader("\n\n\n-1\n");
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains(
            $"Enter the minimum number of digits to be included (default {Constants.MinimumPasswordDigitsDefault}):"));
        Assert.IsTrue(output.Contains(
            "The minimum number of digits must be a non-negative number and cannot exceed the password length."));

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptMinimumDigits_WithExceedingCount_ShouldDisplayError()
    {
        // Arrange
        const int passwordLength = 10;
        const int minimumDigits = 11;

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleInput = new StringReader($"{passwordLength}\n\n\n{minimumDigits}\n");
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains(
            $"Enter the minimum number of digits to be included (default {Constants.MinimumPasswordDigitsDefault}):"));
        Assert.IsTrue(output.Contains(
            "The minimum number of digits must be a non-negative number and cannot exceed the password length."));

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptMinimumSpecialCharacters_WithValidInput_ShouldContinue()
    {
        // Arrange
        const int minimumSpecialCharacters = 2;

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.Is<PasswordGeneratorOptions>(y =>
                y.MinimumSpecialCharacters == minimumSpecialCharacters)))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleInput = new StringReader($"\n\n\n\n{minimumSpecialCharacters}\n");
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains(
            $"Enter the minimum number of special characters to be included " +
            $"(default {Constants.MinimumSpecialPasswordCharactersDefault}):"));

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptMinimumSpecialCharacters_WithEmptyInput_ShouldContinueWithDefault()
    {
        // Arrange
        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.Is<PasswordGeneratorOptions>(y =>
                y.MinimumSpecialCharacters == Constants.MinimumSpecialPasswordCharactersDefault)))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleInput = new StringReader("\n\n\n\n\n");
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains(
            $"Enter the minimum number of special characters to be included " +
            $"(default {Constants.MinimumSpecialPasswordCharactersDefault}):"));

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

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleInput = new StringReader($"{passwordLength}\n\n\n{minimumDigits}\n{minimumSpecialCharacters}\n");
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains(
            "Special character input skipped, there is no room left for additional special characters."));

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptMinimumSpecialCharacters_WithLetterInput_ShouldDisplayError()
    {
        // Arrange
        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleInput = new StringReader($"\n\n\n\nhaha money printer go brr\n");
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains(
            $"Enter the minimum number of special characters to be included " +
            $"(default {Constants.MinimumSpecialPasswordCharactersDefault}):"));
        Assert.IsTrue(output.Contains(
            "The minimum number of special characters must be a non-negative number and the combined total of " +
            "special characters and digits cannot exceed the password's length."));

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptMinimumSpecialCharacters_WithNegativeInput_ShouldDisplayError()
    {
        // Arrange
        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleInput = new StringReader("\n\n\n\n-1\n");
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains(
            $"Enter the minimum number of special characters to be included " +
            $"(default {Constants.MinimumSpecialPasswordCharactersDefault}):"));
        Assert.IsTrue(output.Contains(
            "The minimum number of special characters must be a non-negative number and the combined total of " +
            "special characters and digits cannot exceed the password's length."));

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptMinimumSpecialCharacters_WithExceedingCount_ShouldDisplayError()
    {
        // Arrange
        const int passwordLength = 10;
        const int minimumDigits = 5;
        const int minimumSpecialCharacters = 6;

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleInput = new StringReader($"{passwordLength}\n\n\n{minimumDigits}\n{minimumSpecialCharacters}\n");
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains(
            $"Enter the minimum number of special characters to be included " +
            $"(default {Constants.MinimumSpecialPasswordCharactersDefault}):"));
        Assert.IsTrue(output.Contains(
            "The minimum number of special characters must be a non-negative number and the combined total of " +
            "special characters and digits cannot exceed the password's length."));

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptYesNo_OnAmbiguous_WithValidInput_ShouldContinue()
    {
        // Arrange
        const bool useAmbiguous = false;

        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.Is<PasswordGeneratorOptions>(y =>
                y.AvoidAmbiguousCharacters == useAmbiguous)))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleInput = new StringReader($"\n\n\n\n\n{useAmbiguous}\n");
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains("Avoid ambiguous characters (yes/NO):"));

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptYesNo_OnAmbiguous_WithEmptyInput_ShouldContinueWithDefault()
    {
        // Arrange
        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.Is<PasswordGeneratorOptions>(y =>
                y.AvoidAmbiguousCharacters == Constants.AvoidAmbiguousCharactersInPasswordDefault)))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleInput = new StringReader("\n\n\n\n\n\n");
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains("Avoid ambiguous characters (yes/NO):"));

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PromptYesNo_OnAmbiguous_WithInvalidInput_ShouldDisplayError()
    {
        // Arrange
        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Once);

        var consoleInput = new StringReader("\n\n\n\n\nmeow\n");
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains("Avoid ambiguous characters (yes/NO):"));
        Assert.IsTrue(output.Contains("Please enter 'yes' (y) or 'no' (n)."));

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_WithInvalidConfiguration_ShouldDisplayError()
    {
        // Arrange
        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Never);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Never);

        var consoleInput = new StringReader("\nno\nno\n0\n0\n\n");
        var consoleOutput = new StringWriter();

        Console.SetIn(consoleInput);
        Console.SetOut(consoleOutput);

        // Act
        _sut.DisplayMainPrompt();

        // Assert
        var output = consoleOutput.ToString();

        Assert.IsTrue(output.Contains(
            "Password options must include at least one of: uppercase letters, lowercase letters, digits, " +
            "or special characters."));

        _passwordGeneratorServiceMock.Verify();
        _clipboardMock.Verify();
    }

    [TestMethod]
    public void DisplayMainPrompt_PasswordGeneratorServiceThrowsException_ShouldDisplayError()
    {
        // Arrange
        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
            .Throws(new Exception());

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Verifiable(Times.Never);

        var consoleInput = new StringReader("\n\n\n\n\n\n\n");
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
        _passwordGeneratorServiceMock
            .Setup(x => x.Generate(It.IsAny<PasswordGeneratorOptions>()))
            .Returns(It.IsAny<string>())
            .Verifiable(Times.Once);

        _clipboardMock
            .Setup(x => x.SetText(It.IsAny<string>()))
            .Throws(new Exception());

        var consoleInput = new StringReader("\n\n\n\n\n\n\n");
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
