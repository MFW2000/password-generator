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

    [TestMethod]
    public void ReadInt_WithNoIntInput_ThrowsFormatException()
    {
        // Arrange
        const string inputString = "not an integer";

        var consoleInput = new StringReader($"{inputString}\n");

        Console.SetIn(consoleInput);

        // Act & Assert
        Assert.ThrowsExactly<FormatException>(() => PromptHelper.ReadInt());
    }

    [TestMethod]
    public void ReadInt_WithAllowEmpty_ReturnsNull()
    {
        // Arrange
        var consoleInput = new StringReader("\n");

        Console.SetIn(consoleInput);

        // Act
        var result = PromptHelper.ReadInt(allowEmpty: true);

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void ReadInt_WithinMinRange_ReturnsInt()
    {
        // Arrange
        const string inputString = "100";
        const int expectedInt = 100;
        const int minRange = 100;

        var consoleInput = new StringReader($"{inputString}\n");

        Console.SetIn(consoleInput);

        // Act
        var result = PromptHelper.ReadInt(minRange: minRange);

        // Assert
        Assert.AreEqual(expectedInt, result);
    }

    [TestMethod]
    public void ReadInt_OutOfMinRange_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        const string inputString = "99";
        const int minRange = 100;

        var consoleInput = new StringReader($"{inputString}\n");

        Console.SetIn(consoleInput);

        // Act & Assert
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => PromptHelper.ReadInt(minRange: minRange));
    }

    [TestMethod]
    public void ReadInt_WithinMaxRange_ReturnsInt()
    {
        // Arrange
        const string inputString = "100";
        const int expectedInt = 100;
        const int maxRange = 100;

        var consoleInput = new StringReader($"{inputString}\n");

        Console.SetIn(consoleInput);

        // Act
        var result = PromptHelper.ReadInt(maxRange: maxRange);

        // Assert
        Assert.AreEqual(expectedInt, result);
    }

    [TestMethod]
    public void ReadInt_OutOfMaxRange_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        const string inputString = "101";
        const int maxRange = 100;

        var consoleInput = new StringReader($"{inputString}\n");

        Console.SetIn(consoleInput);

        // Act & Assert
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => PromptHelper.ReadInt(maxRange: maxRange));
    }
}
