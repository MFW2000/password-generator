namespace MFW.PasswordGenerator.Exceptions;

/// <summary>
/// The exception that is thrown when a custom application-specific exception occurs.
/// </summary>
public class PasswordGeneratorException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PasswordGeneratorException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The error message that explains the reason for the exception.</param>
    public PasswordGeneratorException(string message) : base(message) { }
}
