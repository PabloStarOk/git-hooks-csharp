namespace GitHooksCsharp.Logging;

// TODO: Implement ILogger interface.
/// <summary>
/// Provides logging functionalities with different log levels and colors.
/// </summary>
public static class Logger
{
    /// <summary>
    /// Logs a user input message in dark blue color.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public static void LogUserInput(string message)
    {
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine(message);
        CleanColor();
    }

    /// <summary>
    /// Logs a success message in dark green color.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public static void LogSuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.Error.WriteLine(message);
        CleanColor();
    }

    /// <summary>
    /// Logs an informational message in white color.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public static void LogInfo(string message)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.Error.WriteLine(message);
        CleanColor();
    }

    /// <summary>
    /// Logs a warning message in dark yellow color.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public static void LogWarning(string message)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Error.WriteLine(message);
        CleanColor();
    }

    /// <summary>
    /// Logs an error message in dark red color and beeps.
    /// </summary>
    /// <param name="message">The message to log.</param>
    public static void LogError(string message)
    {
        Console.ForegroundColor = ConsoleColor.DarkRed;
        Console.Error.WriteLine(message);
        Console.Beep();
        CleanColor();
    }

    /// <summary>
    /// Logs a message with a specified console color.
    /// </summary>
    /// <param name="consoleColor">The color to use for the message.</param>
    /// <param name="message">The message to log.</param>
    public static void Log(ConsoleColor consoleColor, string message)
    {
        Console.ForegroundColor = consoleColor;
        Console.WriteLine(message);
        CleanColor();
    }

    /// <summary>
    /// Logs a blank line.
    /// </summary>
    public static void BlankLine()
    {
        Console.Error.WriteLine();
    }

    /// <summary>
    /// Resets the console color to white.
    /// </summary>
    private static void CleanColor()
    {
        Console.ForegroundColor = ConsoleColor.White;
    }
}