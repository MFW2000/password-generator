using MFW.PasswordGenerator.Infrastructure.Interfaces;

namespace MFW.PasswordGenerator.Infrastructure;

/// <summary>
/// Implements <see cref="IConsoleClear"/> for testable console clearing.
/// </summary>
public class ConsoleClear : IConsoleClear
{
    /// <inheritdoc/>
    public void Clear()
    {
        Console.Clear();
    }
}
