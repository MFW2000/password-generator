using System.Reflection;
using JetBrains.Annotations;
using MFW.PasswordGenerator.Providers;
using Moq;

namespace MFW.PasswordGeneratorTests.Providers;

[TestClass, UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class AssemblyVersionProviderTests
{
    private Mock<Assembly> _assemblyMock = null!;

    private AssemblyVersionProvider _sut = null!;

    [TestInitialize]
    public void Initialize()
    {
        _assemblyMock = new Mock<Assembly>(MockBehavior.Strict);

        _sut = new AssemblyVersionProvider(_assemblyMock.Object);
    }

    [TestMethod]
    public void GetVersion_WithFoundVersion_ReturnsVersion()
    {
        // Arrange
        var version = new Version(1, 2, 3);
        var assemblyName = new AssemblyName
        {
            Version = version
        };

        _assemblyMock
            .Setup(a => a.GetName())
            .Returns(assemblyName)
            .Verifiable(Times.Once);

        // Act
        var result = _sut.GetVersion();

        // Assert
        Assert.AreEqual(version, result);

        _assemblyMock.Verify();
    }

    [TestMethod]
    public void GetVersion_WithNoFoundVersion_ReturnsDefaultVersion()
    {
        // Arrange
        var assemblyName = new AssemblyName
        {
            Version = null
        };

        _assemblyMock
            .Setup(a => a.GetName())
            .Returns(assemblyName)
            .Verifiable(Times.Once);

        // Act
        var result = _sut.GetVersion();

        // Assert
        Assert.AreEqual(new Version(0,0,0), result);

        _assemblyMock.Verify();
    }
}
