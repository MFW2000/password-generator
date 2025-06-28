using MFW.PasswordGenerator.Helpers;

namespace MFW.PasswordGeneratorTests.Helpers;

[TestClass]
public class PromptHelperTests
{
    [TestMethod]
    public void ReadString_WithAllowEmpty_ReturnsEmptyString()
    {
        // Arrange
        var expectedString = string.Empty;

        var consoleInput = new StringReader("\n");

        Console.SetIn(consoleInput);

        // Act
        var result = PromptHelper.ReadString(allowEmpty: true);

        // Assert
        Assert.AreEqual(expectedString, result);
    }

    [TestMethod]
    public void ReadString_WithoutAllowEmpty_ThrowsArgumentException()
    {
        // Arrange
        var consoleInput = new StringReader("\n");

        Console.SetIn(consoleInput);

        // Act & Assert
        Assert.ThrowsExactly<ArgumentException>(() => PromptHelper.ReadString(allowEmpty: false));
    }

    [TestMethod]
    public void ReadString_WithTrim_ReturnsTrimmedString()
    {
        // Arrange
        const string inputString = " untrimmed string ";
        const string expectedString = "untrimmed string";

        var consoleInput = new StringReader($"{inputString}\n");

        Console.SetIn(consoleInput);

        // Act
        var result = PromptHelper.ReadString(trim: true);

        // Assert
        Assert.AreEqual(expectedString, result);
    }

    [TestMethod]
    public void ReadString_WithoutTrim_ReturnsUntrimmedString()
    {
        // Arrange
        const string inputExpectedString = " untrimmed string ";

        var consoleInput = new StringReader($"{inputExpectedString}\n");

        Console.SetIn(consoleInput);

        // Act
        var result = PromptHelper.ReadString(trim: false);

        // Assert
        Assert.AreEqual(inputExpectedString, result);
    }

    [TestMethod]
    public void ReadString_WithinMaxLength_ReturnsString()
    {
        // Arrange
        const string inputExpectedString = "12345";
        const int maxLength = 5;

        var consoleInput = new StringReader($"{inputExpectedString}\n");

        Console.SetIn(consoleInput);

        // Act
        var result = PromptHelper.ReadString(maxLength: maxLength);

        // Assert
        Assert.AreEqual(inputExpectedString, result);
    }

    [TestMethod]
    public void ReadString_OverMaxLength_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        const string inputString = "123456";
        const int maxLength = 5;

        var consoleInput = new StringReader($"{inputString}\n");

        Console.SetIn(consoleInput);

        // Act & Assert
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => PromptHelper.ReadString(maxLength: maxLength));
    }
}
