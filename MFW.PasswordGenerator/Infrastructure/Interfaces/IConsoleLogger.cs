namespace MFW.PasswordGenerator.Infrastructure.Interfaces;

public interface IConsoleLogger
{
    /// <summary>
    /// Logs a debug message to the log file.
    /// </summary>
    /// <param name="message">
    /// Format the string of the log message in the message template format.
    /// Example: <c>"User {User} logged in from {Address}"</c>.
    /// </param>
    /// <param name="logFile">Optional log file path. Defaults to <see cref="Constants.DefaultLogFile"/>.</param>
    void LogDebug(string message, string logFile = Constants.DefaultLogFile);

    /// <summary>
    /// Logs an information message to the log file.
    /// </summary>
    /// <param name="message">
    /// Format the string of the log message in the message template format.
    /// Example: <c>"User {User} logged in from {Address}"</c>.
    /// </param>
    /// <param name="logFile">Optional log file path. Defaults to <see cref="Constants.DefaultLogFile"/>.</param>
    void LogInformation(string message, string logFile = Constants.DefaultLogFile);

    /// <summary>
    /// Logs a warning message to the log file.
    /// </summary>
    /// <param name="message">
    /// Format the string of the log message in the message template format.
    /// Example: <c>"User {User} logged in from {Address}"</c>.
    /// </param>
    /// <param name="logFile">Optional log file path. Defaults to <see cref="Constants.DefaultLogFile"/>.</param>
    void LogWarning(string message, string logFile = Constants.DefaultLogFile);

    /// <summary>
    /// Logs an error message to the log file.
    /// </summary>
    /// <param name="message">
    /// Format the string of the log message in the message template format.
    /// Example: <c>"User {User} logged in from {Address}"</c>.
    /// </param>
    /// <param name="logFile">Optional log file path. Defaults to <see cref="Constants.DefaultLogFile"/>.</param>
    void LogError(string message, string logFile = Constants.DefaultLogFile);

    /// <summary>
    /// Logs a fatal message to the log file.
    /// </summary>
    /// <param name="message">
    /// Format the string of the log message in the message template format.
    /// Example: <c>"User {User} logged in from {Address}"</c>.
    /// </param>
    /// <param name="logFile">Optional log file path. Defaults to <see cref="Constants.DefaultLogFile"/>.</param>
    void LogCritical(string message, string logFile = Constants.DefaultLogFile);
}
