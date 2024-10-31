namespace GitHooksCsharp.Cmd;

/// <summary>
/// Represents the result of a command execution, including standard output, exit code, and error output.
/// </summary>
/// <param name="standardOutput">The standard output of the command.</param>
/// <param name="exitCode">The exit code of the command.</param>
/// <param name="errorOutput">The error output of the command. Defaults to an empty string.</param>
public class CmdResult(string standardOutput, int exitCode, string errorOutput = "", string path = "") : BaseResult<string>(standardOutput, exitCode, errorOutput, path)
{
    /// <summary>
    /// Formats the standard output by splitting it into a list of strings based on new lines.
    /// </summary>
    /// <param name="standardOutput">The standard output string to be formatted.</param>
    protected override void FormatOutput(string standardOutput)
    {
        Outputs = standardOutput.Split(Environment.NewLine).ToList();
    }

    /// <summary>
    /// Formats the error output by splitting it into a list of strings based on new lines.
    /// </summary>
    /// <param name="errorOutput">The error output string to be formatted.</param>
    protected override void FormatErrors(string errorOutput)
    {
        Errors = errorOutput.Split(Environment.NewLine).ToList();
    }
}