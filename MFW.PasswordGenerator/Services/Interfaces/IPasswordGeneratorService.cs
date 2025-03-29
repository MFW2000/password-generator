using MFW.PasswordGenerator.Records;

namespace MFW.PasswordGenerator.Services.Interfaces;

public interface IPasswordGeneratorService
{
    string Generate(PasswordGeneratorOptions options);
}
