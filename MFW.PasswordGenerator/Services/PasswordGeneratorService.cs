using System.Text.RegularExpressions;
using MFW.PasswordGenerator.Exceptions;
using MFW.PasswordGenerator.Records;
using MFW.PasswordGenerator.Services.Interfaces;

namespace MFW.PasswordGenerator.Services;

public partial class PasswordGeneratorService : IPasswordGeneratorService
{
    [GeneratedRegex("[lIO01]")]
    private static partial Regex AmbiguousCharactersRegex();

    public string Generate(PasswordGeneratorOptions options)
    {
        if (options.Length is < 5 or > 128)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options),
                "Password length must be between 5 and 128 characters.");
        }

        if (options.MinimumNumbers < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options),
                "Minimum number count cannot be a negative number.");
        }

        if (options.MinimumSpecialCharacters < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(options),
                "Minimum special characters cannot be a negative number.");
        }

        if (options.MinimumNumbers + options.MinimumSpecialCharacters > options.Length)
        {
            throw new ArgumentException("Cannot have more numbers or special characters than the password length.");
        }

        var isInvalidPasswordConfig = options is
        {
            IncludeUppercase: false,
            IncludeLowercase: false,
            MinimumNumbers: 0,
            MinimumSpecialCharacters: 0
        };

        if (isInvalidPasswordConfig)
        {
            throw new ArgumentException(
                "Password options must include at least one of: uppercase letters, lowercase letters, numbers, or " +
                "special characters.");
        }

        var characters = GetPasswordCharacters(options);
        var password = RandomizePassword(options.Length, characters);

        if (string.IsNullOrEmpty(password))
        {
            throw new PasswordGeneratorException("Something went wrong while generating the password.");
        }

        return password;
    }

    private static string GetPasswordCharacters(PasswordGeneratorOptions options)
    {
        var characters = string.Empty;

        if (options.IncludeUppercase)
        {
            characters += "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        }

        if (options.IncludeLowercase)
        {
            characters += "abcdefghijklmnopqrstuvwxyz";
        }

        if (options.MinimumNumbers > 0)
        {
            characters += "0123456789";
        }

        if (options.MinimumSpecialCharacters > 0)
        {
            characters += "!@#$%^&*";
        }

        if (options.AvoidAmbiguousCharacters)
        {
            characters = AmbiguousCharactersRegex().Replace(characters, "");
        }

        return characters;
    }

    private static string RandomizePassword(int length, string characters)
    {
        var random = new Random();
        var randomizedPasswordChars = Enumerable
            .Repeat(characters, length)
            .Select(s => s[random.Next(s.Length)])
            .ToArray();

        return string.Join("", randomizedPasswordChars);
    }
}
