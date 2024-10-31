using System.Diagnostics;
using System.Runtime.InteropServices;

namespace GitHooksCsharp.Cmd;

/// <summary>
/// Provides methods to execute commands using PowerShell.
/// </summary>
public static class CommandLine
{
    // TODO: Allow the user to specify the CLI to use.
    private readonly static string Cli = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "powershell.exe" : "/bin/bash";

    /// <summary>
    /// Executes a given command using PowerShell and returns the result.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    /// <param name="path"></param>
    /// <returns>A <see cref="CmdResult"/> containing the standard output, exit code, and error output.</returns>
    public static CmdResult Execute(string command, string path = "")
    {
        command = $"{command} {path}";
        command = command.Replace("\"", "\"\"").Trim();

        using Process proc = new();
        proc.StartInfo = new ProcessStartInfo
        {
            FileName = Cli,
            Arguments = "-c \"" + command + "\"",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
        };
        
        proc.Start();
        proc.WaitForExit();

        string stdResult = proc.StandardOutput.ReadToEnd().Trim();
        string errResult = proc.StandardError.ReadToEnd().Trim();
        int exitCode = proc.ExitCode;

        if (exitCode != 0 && string.IsNullOrEmpty(errResult))
            errResult = stdResult;

        return new CmdResult(stdResult, exitCode, errResult, path);
    }
    
    /// <summary>
    /// Executes a given command asynchronously using PowerShell and returns the result.
    /// </summary>
    /// <param name="command">The command to execute.</param>
    /// <param name="path">The path to execute the command in. Default is an empty string.</param>
    /// <returns>A <see cref="CmdResult"/> containing the standard output, exit code, and error output.</returns>
    public static async Task<CmdResult> ExecuteAsync(string command, string path = "")
    {
        command = $"{command} {path}";
        command = command.Replace("\"", "\"\"").Trim();

        using Process proc = new();
        proc.StartInfo = new ProcessStartInfo
        {
            FileName = Cli,
            Arguments = "-c \"" + command + "\"",
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true,
        };
        
        proc.Start();
        await proc.WaitForExitAsync();

        string stdResult = await proc.StandardOutput.ReadToEndAsync();
        string errResult = await proc.StandardError.ReadToEndAsync();
        int exitCode = proc.ExitCode;

        if (exitCode != 0 && string.IsNullOrEmpty(errResult))
            errResult = stdResult;
        
        return new CmdResult(stdResult.Trim(), exitCode, errResult.Trim(), path);
    }
}
