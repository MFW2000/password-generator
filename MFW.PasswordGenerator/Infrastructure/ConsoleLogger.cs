using MFW.PasswordGenerator.Infrastructure.Interfaces;

namespace MFW.PasswordGenerator.Infrastructure;

public class ConsoleLogger : IConsoleLogger
{
    private static readonly object Lock = new();

    /// <inheritdoc/>
    public void LogDebug(string message, string logFile = Constants.DefaultLogFile)
    {
        Log("DBG", message, logFile);
    }

    /// <inheritdoc/>
    public void LogInformation(string message, string logFile = Constants.DefaultLogFile)
    {
        Log("INF", message, logFile);
    }

    /// <inheritdoc/>
    public void LogWarning(string message, string logFile = Constants.DefaultLogFile)
    {
        Log("WRN", message, logFile);
    }

    /// <inheritdoc/>
    public void LogError(string message, string logFile = Constants.DefaultLogFile)
    {
        Log("ERR", message, logFile);
    }

    /// <inheritdoc/>
    public void LogCritical(string message, string logFile = Constants.DefaultLogFile)
    {
        Log("FTL", message, logFile);
    }

    private static void Log(string level, string message, string logFile)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            logFile = Constants.DefaultLogFile;
        }

        var timestamp = DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss");
        var logEntry = $"[{timestamp} {level}] {message}";

        lock (Lock)
        {
            File.AppendAllText(logFile, logEntry + Environment.NewLine);
        }
    }
}
