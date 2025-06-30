using MFW.PasswordGenerator;
using MFW.PasswordGenerator.Exceptions;
using MFW.PasswordGenerator.Records;
using MFW.PasswordGenerator.Services;

namespace MFW.PasswordGeneratorTests.Services;

[TestClass]
public class PasswordGeneratorServiceTests
{
    private PasswordGeneratorService _sut = null!;

    [TestInitialize]
    public void Initialize()
    {
        _sut = new PasswordGeneratorService();
    }

    [TestMethod]
    public void Generate_WithoutLength_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var options = GetOptions(0);

        // Act & Assert
        Assert.ThrowsExactly<PasswordGeneratorException>(() => _sut.Generate(options));
    }

    [TestMethod]
    public void Generate_WithoutUppercaseOrLowercase_ThrowsArgumentException()
    {
        // Arrange
        var options = GetOptions();

        // Act & Assert
        Assert.ThrowsExactly<PasswordGeneratorException>(() => _sut.Generate(options));
    }

    [TestMethod]
    public void Generate_WithSpecifiedLength_ReturnsPassword()
    {
        // Arrange
        const int length = 14;

        var options = GetOptions(length: length, useUppercase: true);

        // Act
        var result = _sut.Generate(options);

        // Assert
        Assert.AreEqual(length, result.Length);
    }

    [TestMethod]
    public void Generate_WithUpperCase_ReturnsPassword()
    {
        // Arrange
        var options = GetOptions(useUppercase: true);

        // Act
        var result = _sut.Generate(options);

        // Assert
        Assert.IsTrue(result.Any(char.IsUpper));
    }

    [TestMethod]
    public void Generate_WithLowerCase_ReturnsPassword()
    {
        // Arrange
        var options = GetOptions(useLowercase: true);

        // Act
        var result = _sut.Generate(options);

        // Assert
        Assert.IsTrue(result.Any(char.IsLower));
    }

    [TestMethod]
    public void Generate_WithMinimumDigits_ReturnsPassword()
    {
        // Arrange
        const int digits = 3;

        var options = GetOptions(minimumDigits: digits);

        // Act
        var result = _sut.Generate(options);

        // Assert
        var digitCharCount = result.Count(char.IsDigit);

        Assert.IsTrue(digitCharCount >= digits);
    }

    [TestMethod]
    public void Generate_WithSpecialCharacters_ReturnsPassword()
    {
        // Arrange
        const int specials = 3;

        var options = GetOptions(minimumSpecialCharacters: specials);

        // Act
        var result = _sut.Generate(options);

        // Assert
        var specialCharCount = result.Count(c => Constants.Special.Contains(c));

        Assert.IsTrue(specialCharCount >= specials);
    }

    [TestMethod]
    public void Generate_WithoutAmbiguousCharacters_ReturnsPassword()
    {
        // Arrange
        var options = GetOptions(
            useUppercase: true,
            useLowercase: true,
            minimumDigits: 1,
            avoidAmbiguousCharacters: true);

        // Act
        var result = _sut.Generate(options);

        // Assert
        var ambiguousCharCount = result.Count(c => Constants.AmbiguousCharacters.Contains(c));

        Assert.AreEqual(0, ambiguousCharCount);
    }

    [TestMethod]
    public void Generate_WithInvalidMinimumLength_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var options = GetOptions(4);

        // Act & Assert
        Assert.ThrowsExactly<PasswordGeneratorException>(() => _sut.Generate(options));
    }

    [TestMethod]
    public void Generate_WithInvalidMaximumLength_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var options = GetOptions(129);

        // Act & Assert
        Assert.ThrowsExactly<PasswordGeneratorException>(() => _sut.Generate(options));
    }

    [TestMethod]
    public void Generate_WithInvalidMinimumDigits_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var options = GetOptions(minimumDigits: -1);

        // Act & Assert
        Assert.ThrowsExactly<PasswordGeneratorException>(() => _sut.Generate(options));
    }

    [TestMethod]
    public void Generate_WithInvalidMinimumSpecialCharacters_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        var options = GetOptions(minimumSpecialCharacters: -1);

        // Act & Assert
        Assert.ThrowsExactly<PasswordGeneratorException>(() => _sut.Generate(options));
    }

    [TestMethod]
    public void Generate_WithMoreDigitsAndSpecialCharactersThanLength_ThrowsArgumentException()
    {
        // Arrange
        var options = GetOptions(length: 10, minimumDigits: 6, minimumSpecialCharacters: 6);

        // Act & Assert
        Assert.ThrowsExactly<PasswordGeneratorException>(() => _sut.Generate(options));
    }

    [TestMethod]
    public void Generate_WithoutAnyOptions_ThrowsArgumentException()
    {
        // Arrange
        var options = GetOptions();

        // Act & Assert
        Assert.ThrowsExactly<PasswordGeneratorException>(() => _sut.Generate(options));
    }

    private static PasswordGeneratorOptions GetOptions(
        int length = 18,
        bool useUppercase = false,
        bool useLowercase = false,
        int minimumDigits = 0,
        int minimumSpecialCharacters = 0,
        bool avoidAmbiguousCharacters = false)
    {
        return new PasswordGeneratorOptions(
            length,
            useUppercase,
            useLowercase,
            minimumDigits,
            minimumSpecialCharacters,
            avoidAmbiguousCharacters);
    }
}
