using MFW.PasswordGenerator.Exceptions;
using MFW.PasswordGenerator.Infrastructure.Utility;
using MFW.PasswordGenerator.Records;
using MFW.PasswordGenerator.Services.Interfaces;

namespace MFW.PasswordGenerator.Services;

/// <summary>
/// Implements <see cref="IPasswordGeneratorService"/> for generating new passwords.
/// </summary>
public class PasswordGeneratorService : IPasswordGeneratorService
{
    /// <inheritdoc/>
    public string Generate(PasswordGeneratorOptions options)
    {
        if (options.Length is < Constants.MinimumPasswordLength or > Constants.MaximumPasswordLength)
        {
            throw new PasswordGeneratorException("Password length must be between 5 and 128 characters.");
        }

        if (options.MinimumDigits < 0)
        {
            throw new PasswordGeneratorException("Minimum digit count cannot be a negative amount.");
        }

        if (options.MinimumSpecialCharacters < 0)
        {
            throw new PasswordGeneratorException("Minimum special characters cannot be a negative amount.");
        }

        if (options.MinimumDigits + options.MinimumSpecialCharacters > options.Length)
        {
            throw new PasswordGeneratorException(
                "Cannot have more digits or special characters than the password length.");
        }

        var isInvalidPasswordConfig = options is
        {
            IncludeUppercase: false,
            IncludeLowercase: false,
            MinimumDigits: 0,
            MinimumSpecialCharacters: 0
        };

        if (isInvalidPasswordConfig)
        {
            throw new PasswordGeneratorException(
                "Password options must include at least one of: uppercase letters, lowercase letters, digits, or " +
                "special characters.");
        }

        var password = RandomizePassword(options);

        return password;
    }

    /// <summary>
    /// Randomizes a new password based on the specified options. To ensure that characters from the selected
    /// categories are included at least once (or as specified by minimum requirements), the password is pre-populated
    /// with the required characters before filling the remaining length with random characters from the allowed pool.
    /// </summary>
    /// <param name="options">The options that specify the password requirements.</param>
    /// <returns>The generated password that meets the specified requirements.</returns>
    private static string RandomizePassword(PasswordGeneratorOptions options)
    {
        var uppercase = Constants.Uppercase;
        var lowercase = Constants.Lowercase;
        var digits = Constants.Digits;
        var random = new Random();
        var passwordCharacters = new List<char>();
        var remainingCharacterPool = string.Empty;

        if (options.AvoidAmbiguousCharacters)
        {
            uppercase = RegexUtility.AmbiguousCharactersRegex().Replace(uppercase, "");
            lowercase = RegexUtility.AmbiguousCharactersRegex().Replace(lowercase, "");
            digits = RegexUtility.AmbiguousCharactersRegex().Replace(digits, "");
        }

        if (options.IncludeUppercase)
        {
            passwordCharacters.Add(uppercase[random.Next(uppercase.Length)]);
            remainingCharacterPool += uppercase;
        }

        if (options.IncludeLowercase)
        {
            passwordCharacters.Add(lowercase[random.Next(lowercase.Length)]);
            remainingCharacterPool += lowercase;
        }

        if (options.MinimumDigits > 0)
        {
            for (var i = 0; i < options.MinimumDigits; i++)
            {
                passwordCharacters.Add(digits[random.Next(digits.Length)]);
            }

            remainingCharacterPool += digits;
        }

        if (options.MinimumSpecialCharacters > 0)
        {
            for (var i = 0; i < options.MinimumSpecialCharacters; i++)
            {
                passwordCharacters.Add(Constants.Special[random.Next(Constants.Special.Length)]);
            }

            remainingCharacterPool += Constants.Special;
        }

        while (passwordCharacters.Count < options.Length)
        {
            passwordCharacters.Add(remainingCharacterPool[random.Next(remainingCharacterPool.Length)]);
        }

        var shuffledPassword = string.Join("", passwordCharacters.OrderBy(_ => random.Next()));

        return shuffledPassword;
    }
}
