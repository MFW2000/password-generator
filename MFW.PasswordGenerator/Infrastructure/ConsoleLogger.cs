using MFW.PasswordGenerator.Infrastructure.Interfaces;

namespace MFW.PasswordGenerator.Infrastructure;

/// <summary>
/// Implements <see cref="IConsoleLogger"/> for logging messages to a log file.
/// </summary>
public class ConsoleLogger(TimeProvider timeProvider) : IConsoleLogger
{
    private static readonly object Lock = new();

    /// <inheritdoc/>
    public void LogDebug(string message, string logFile = Constants.DefaultLogFile)
    {
        Log(LogLevel.Dbg, message, logFile);
    }

    /// <inheritdoc/>
    public void LogInformation(string message, string logFile = Constants.DefaultLogFile)
    {
        Log(LogLevel.Inf, message, logFile);
    }

    /// <inheritdoc/>
    public void LogWarning(string message, string logFile = Constants.DefaultLogFile)
    {
        Log(LogLevel.Wrn, message, logFile);
    }

    /// <inheritdoc/>
    public void LogError(string message, string logFile = Constants.DefaultLogFile)
    {
        Log(LogLevel.Err, message, logFile);
    }

    /// <inheritdoc/>
    public void LogCritical(string message, string logFile = Constants.DefaultLogFile)
    {
        Log(LogLevel.Ftl, message, logFile);
    }

    /// <summary>
    /// Writes a log entry to a specified log file with a specified log level and message.
    /// </summary>
    /// <param name="level">The severity level of the log message.</param>
    /// <param name="message">The message to log.</param>
    /// <param name="logFile">The path of the log file where the entry will be written.</param>
    private void Log(LogLevel level, string message, string logFile)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            return;
        }

        var logLevelString = level.ToString().ToUpper();
        var timestamp = timeProvider.GetLocalNow().ToString("yyyy-MM-dd HH:mm:ss");
        var logEntry = $"[{timestamp} {logLevelString}] {message}";

        lock (Lock)
        {
            File.AppendAllText(logFile, logEntry + Environment.NewLine);
        }
    }

    /// <summary>
    /// Represents the severity of log messages.
    /// </summary>
    private enum LogLevel
    {
        Dbg,
        Inf,
        Wrn,
        Err,
        Ftl
    }
}
