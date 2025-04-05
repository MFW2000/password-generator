namespace MFW.PasswordGenerator.Records;

public record PasswordGeneratorOptions(
    int Length,
    bool IncludeUppercase,
    bool IncludeLowercase,
    int MinimumDigits,
    int MinimumSpecialCharacters,
    bool AvoidAmbiguousCharacters);
