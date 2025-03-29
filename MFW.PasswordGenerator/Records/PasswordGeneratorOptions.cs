namespace MFW.PasswordGenerator.Records;

public record PasswordGeneratorOptions(
    int Length,
    bool IncludeUppercase,
    bool IncludeLowercase,
    int MinimumNumbers,
    int MinimumSpecialCharacters,
    bool AvoidAmbiguousCharacters);
