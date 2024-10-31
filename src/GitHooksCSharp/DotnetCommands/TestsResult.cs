namespace GitHooksCsharp.DotnetCommands;

/// <summary>
/// Represents the result of running tests, including standard output, exit code, error output, and path.
/// </summary>
/// <param name="standardOutput">The standard output from the test run.</param>
/// <param name="exitCode">The exit code from the test run.</param>
/// <param name="errorOutput">The error output from the test run (optional).</param>
/// <param name="path">The path related to the test run (optional).</param>
public class TestsResult(string standardOutput, int exitCode, string errorOutput = "", string path = "") : BaseResult<string>(standardOutput, exitCode, errorOutput, path)
{
    /// <summary>
    /// Formats the standard output by splitting it into a list of strings.
    /// </summary>
    /// <param name="standardOutput">The standard output to format.</param>
    protected override void FormatOutput(string standardOutput)
    {
        Outputs = standardOutput.Split("\n").ToList();
    }

    /// <summary>
    /// Formats the error output by splitting it into a list of strings.
    /// </summary>
    /// <param name="errorOutput">The error output to format.</param>
    protected override void FormatErrors(string errorOutput)
    {
        Errors = errorOutput.Split("\n").ToList();
    }
}