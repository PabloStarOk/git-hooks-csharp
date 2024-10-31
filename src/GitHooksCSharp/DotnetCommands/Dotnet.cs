using GitHooksCsharp.Cmd;

namespace GitHooksCsharp.DotnetCommands;

// TODO: Allow to use parameters for the commands.
/// <summary>
/// Provides methods to execute various dotnet commands.
/// </summary>
public static class Dotnet
{
    /// <summary>
    /// Formats the code in the specified path.
    /// </summary>
    /// <param name="path">The path to the code to format.</param>
    /// <returns>The result of the format command.</returns>
    public static CmdResult FormatCode(string path = "")
    {
        return CommandLine.Execute("dotnet format", path);
    }
    
    /// <summary>
    /// Asynchronously formats the code in the specified path.
    /// </summary>
    /// <param name="path">The path to the code to format.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the result of the format command.</returns>
    public static async Task<CmdResult> FormatCodeAsync(string path = "")
    {
        return await CommandLine.ExecuteAsync("dotnet format", path);
    }

    /// <summary>
    /// Cleans the code in the specified path.
    /// </summary>
    /// <param name="path">The path to the code to clean.</param>
    /// <returns>The result of the clean command.</returns>
    public static CmdResult CleanCode(string path = "")
    {
        return CommandLine.Execute("dotnet clean", path);
    }
    
    /// <summary>
    /// Asynchronously cleans the code in the specified path.
    /// </summary>
    /// <param name="path">The path to the code to clean.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the result of the clean command.</returns>
    public static async Task<CmdResult> CleanCodeAsync(string path = "")
    {
        return await CommandLine.ExecuteAsync($"dotnet clean", path);
    }

    /// <summary>
    /// Builds the code in the specified path.
    /// </summary>
    /// <param name="path">The path to the code to build.</param>
    /// <returns>The result of the build command.</returns>
    public static BuildResult BuildCode(string path = "")
    {
        CmdResult result = CommandLine.Execute("dotnet build", path);
        BuildResult buildResult = new(result.StandardOutput, result.ExitCode, result.ErrorOutput, result.Path);

        return buildResult;
    }
    
    /// <summary>
    /// Asynchronously builds the code in the specified path.
    /// </summary>
    /// <param name="path">The path to the code to build.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the result of the build command.</returns>
    public static async Task<BuildResult> BuildCodeAsync(string path = "")
    {
        CmdResult result = await CommandLine.ExecuteAsync("dotnet build", path);
        BuildResult buildResult = new(result.StandardOutput, result.ExitCode, result.ErrorOutput, result.Path);

        return buildResult;
    }

    /// <summary>
    /// Tests the code in the specified path.
    /// </summary>
    /// <param name="path">The path to the code to test.</param>
    /// <returns>The result of the test command.</returns>
    public static TestsResult TestCode(string path = "")
    {
        CmdResult result = CommandLine.Execute("dotnet test", path);
        TestsResult testResult = new(result.StandardOutput, result.ExitCode, result.ErrorOutput, result.Path);

        return testResult;
    }
    
    /// <summary>
    /// Asynchronously tests the code in the specified path.
    /// </summary>
    /// <param name="path">The path to the code to test.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains the result of the test command.</returns>
    public static async Task<TestsResult> TestCodeAsync(string path = "")
    {
        CmdResult result = await CommandLine.ExecuteAsync("dotnet test", path);
        TestsResult testResult = new(result.StandardOutput, result.ExitCode, result.ErrorOutput, result.Path);

        return testResult;
    }
}