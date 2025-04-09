using System.Text.RegularExpressions;

namespace MFW.PasswordGenerator.Infrastructure.Utility;

/// <summary>
/// Provides utility methods and regular expressions for string manipulation and pattern matching.
/// </summary>
public static partial class RegexUtility
{
    [GeneratedRegex($"[{Constants.AmbiguousCharacters}]")]
    public static partial Regex AmbiguousCharactersRegex();
}
