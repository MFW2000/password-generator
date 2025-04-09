namespace MFW.PasswordGenerator.Records;

/// <summary>
/// Represents options for generating a password with specific requirements.
/// </summary>
/// <param name="Length">The total length of the password to generate.</param>
/// <param name="IncludeUppercase">Indicates whether to include uppercase letters.</param>
/// <param name="IncludeLowercase">Indicates whether to include lowercase letters.</param>
/// <param name="MinimumDigits">The minimum number of digits to include.</param>
/// <param name="MinimumSpecialCharacters">The minimum number of special characters to include.</param>
/// <param name="AvoidAmbiguousCharacters">
/// Indicates whether to exclude ambiguous characters that can be easily confused.
/// </param>
public record PasswordGeneratorOptions(
    int Length,
    bool IncludeUppercase,
    bool IncludeLowercase,
    int MinimumDigits,
    int MinimumSpecialCharacters,
    bool AvoidAmbiguousCharacters);
