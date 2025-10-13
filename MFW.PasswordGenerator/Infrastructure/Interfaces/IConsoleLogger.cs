namespace MFW.PasswordGenerator.Infrastructure.Interfaces;

/// <summary>
/// Provides a contract for logging messages to a log file.
/// </summary>
public interface IConsoleLogger
{
    /// <summary>
    /// Logs a debug message to the log file.
    /// </summary>
    /// <param name="message">The debug message to log.</param>
    /// <param name="logFile">Optional log file path. Defaults to <see cref="Constants.DefaultLogFile"/>.</param>
    void LogDebug(string message, string logFile = Constants.DefaultLogFile);

    /// <summary>
    /// Logs an information message to the log file.
    /// </summary>
    /// <param name="message">The information message to log.</param>
    /// <param name="logFile">Optional log file path. Defaults to <see cref="Constants.DefaultLogFile"/>.</param>
    void LogInformation(string message, string logFile = Constants.DefaultLogFile);

    /// <summary>
    /// Logs a warning message to the log file.
    /// </summary>
    /// <param name="message">The warning message to log.</param>
    /// <param name="logFile">Optional log file path. Defaults to <see cref="Constants.DefaultLogFile"/>.</param>
    void LogWarning(string message, string logFile = Constants.DefaultLogFile);

    /// <summary>
    /// Logs an error message to the log file.
    /// </summary>
    /// <param name="message">The error message to log.</param>
    /// <param name="logFile">Optional log file path. Defaults to <see cref="Constants.DefaultLogFile"/>.</param>
    void LogError(string message, string logFile = Constants.DefaultLogFile);

    /// <summary>
    /// Logs a fatal (critical) message to the log file.
    /// </summary>
    /// <param name="message">The fatal message to log.</param>
    /// <param name="logFile">Optional log file path. Defaults to <see cref="Constants.DefaultLogFile"/>.</param>
    void LogCritical(string message, string logFile = Constants.DefaultLogFile);
}
