using MFW.PasswordGenerator.Exceptions;
using MFW.PasswordGenerator.Infrastructure.Utility;
using MFW.PasswordGenerator.Records;
using MFW.PasswordGenerator.Services.Interfaces;

namespace MFW.PasswordGenerator.Services;

public class PasswordGeneratorService : IPasswordGeneratorService
{
    private const string Special = "!@#$%^&*";

    private static string _uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private static string _lowercase = "abcdefghijklmnopqrstuvwxyz";
    private static string _digits = "0123456789";

    public string Generate(PasswordGeneratorOptions options)
    {
        if (options.Length is < 5 or > 128)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options),
                "Password length must be between 5 and 128 characters.");
        }

        if (options.MinimumDigits < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options),
                "Minimum digit count cannot be a negative amount.");
        }

        if (options.MinimumSpecialCharacters < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options),
                "Minimum special characters cannot be a negative amount.");
        }

        if (options.MinimumDigits + options.MinimumSpecialCharacters > options.Length)
        {
            throw new ArgumentException("Cannot have more digits or special characters than the password length.");
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
            throw new ArgumentException(
                "Password options must include at least one of: uppercase letters, lowercase letters, digits, or " +
                "special characters.");
        }

        var password = RandomizePassword(options);

        if (string.IsNullOrEmpty(password))
        {
            throw new PasswordGeneratorException("Something went wrong while generating the password.");
        }

        return password;
    }

    // TODO: Perhaps add comments or extract code to clarify what is happening.

    // pre-populate the password with the required characters before filling the remaining length randomly.

    private static string RandomizePassword(PasswordGeneratorOptions options)
    {
        var random = new Random();
        var passwordCharacters = new List<char>();
        var remainingCharacterPool = string.Empty;

        if (options.AvoidAmbiguousCharacters)
        {
            _uppercase = Regex.AmbiguousCharactersRegex().Replace(_uppercase, "");
            _lowercase = Regex.AmbiguousCharactersRegex().Replace(_lowercase, "");
            _digits = Regex.AmbiguousCharactersRegex().Replace(_digits, "");
        }

        if (options.IncludeUppercase)
        {
            passwordCharacters.Add(_uppercase[random.Next(_uppercase.Length)]);
            remainingCharacterPool += _uppercase;
        }

        if (options.IncludeLowercase)
        {
            passwordCharacters.Add(_lowercase[random.Next(_lowercase.Length)]);
            remainingCharacterPool += _lowercase;
        }

        if (options.MinimumDigits > 0)
        {
            for (var i = 0; i < options.MinimumDigits; i++)
            {
                passwordCharacters.Add(_digits[random.Next(_digits.Length)]);
            }

            remainingCharacterPool += _digits;
        }

        if (options.MinimumSpecialCharacters > 0)
        {
            for (var i = 0; i < options.MinimumSpecialCharacters; i++)
            {
                passwordCharacters.Add(Special[random.Next(Special.Length)]);
            }

            remainingCharacterPool += Special;
        }

        while (passwordCharacters.Count < options.Length)
        {
            passwordCharacters.Add(remainingCharacterPool[random.Next(remainingCharacterPool.Length)]);
        }

        var shuffledPassword = string.Join("", passwordCharacters.OrderBy(_ => random.Next()));

        return shuffledPassword;
    }
}
