using JetBrains.Annotations;
using MFW.PasswordGenerator.Helpers;

namespace MFW.PasswordGeneratorTests.Helpers;

[TestClass, UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class PromptHelperTests
{
    [TestMethod]
    public void ReadString_WithAllowEmpty_ReturnsEmptyString()
    {
        // Arrange
        const string input = "\n";

        var expectedString = string.Empty;

        var consoleInput = new StringReader(input);

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
        const string input = "\n";

        var consoleInput = new StringReader(input);

        Console.SetIn(consoleInput);

        // Act & Assert
        Assert.ThrowsExactly<ArgumentException>(() => PromptHelper.ReadString(allowEmpty: false));
    }

    [TestMethod]
    public void ReadString_WithTrim_ReturnsTrimmedString()
    {
        // Arrange
        const string input = " untrimmed string \n";
        const string expectedString = "untrimmed string";

        var consoleInput = new StringReader(input);

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
        const string input = " untrimmed string \n";
        const string expectedString = " untrimmed string ";

        var consoleInput = new StringReader(input);

        Console.SetIn(consoleInput);

        // Act
        var result = PromptHelper.ReadString(trim: false);

        // Assert
        Assert.AreEqual(expectedString, result);
    }

    [TestMethod]
    public void ReadString_WithinMaxLength_ReturnsString()
    {
        // Arrange
        const string input = "12345\n";
        const string expectedString = "12345";
        const int maxLength = 5;

        var consoleInput = new StringReader(input);

        Console.SetIn(consoleInput);

        // Act
        var result = PromptHelper.ReadString(maxLength: maxLength);

        // Assert
        Assert.AreEqual(expectedString, result);
    }

    [TestMethod]
    public void ReadString_OverMaxLength_ThrowsArgumentOutOfRangeException()
    {
        // Arrange
        const string input = "123456\n";
        const int maxLength = 5;

        var consoleInput = new StringReader(input);

        Console.SetIn(consoleInput);

        // Act & Assert
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => PromptHelper.ReadString(maxLength: maxLength));
    }

    [TestMethod]
    public void ReadInt_WithNoIntInput_ThrowsFormatException()
    {
        // Arrange
        const string input = "not an integer\n";

        var consoleInput = new StringReader(input);

        Console.SetIn(consoleInput);

        // Act & Assert
        Assert.ThrowsExactly<FormatException>(() => PromptHelper.ReadInt());
    }

    [TestMethod]
    public void ReadInt_WithAllowEmpty_ReturnsNull()
    {
        // Arrange
        const string input = "\n";

        var consoleInput = new StringReader(input);

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
        const string input = "100\n";
        const int expectedInt = 100;
        const int minRange = 100;

        var consoleInput = new StringReader(input);

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
        const string input = "99\n";
        const int minRange = 100;

        var consoleInput = new StringReader(input);

        Console.SetIn(consoleInput);

        // Act & Assert
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => PromptHelper.ReadInt(minRange: minRange));
    }

    [TestMethod]
    public void ReadInt_WithinMaxRange_ReturnsInt()
    {
        // Arrange
        const string input = "100\n";
        const int expectedInt = 100;
        const int maxRange = 100;

        var consoleInput = new StringReader(input);

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
        const string input = "101\n";
        const int maxRange = 100;

        var consoleInput = new StringReader(input);

        Console.SetIn(consoleInput);

        // Act & Assert
        Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => PromptHelper.ReadInt(maxRange: maxRange));
    }
}
