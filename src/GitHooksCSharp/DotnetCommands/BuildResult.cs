namespace GitHooksCsharp.DotnetCommands;

/// <summary>
/// Represents the result of a build process.
/// </summary>
/// <param name="standardOutput">The standard output from the build process.</param>
/// <param name="exitCode">The exit code of the build process.</param>
/// <param name="errorOutput">The error output from the build process.</param>
/// <param name="path">The path related to the build process.</param>
public class BuildResult(string standardOutput, int exitCode, string errorOutput = "", string path = "") : BaseResult<string>(standardOutput, exitCode, errorOutput, path)
{
    /// <summary>
    /// Formats the standard output by splitting it into lines.
    /// </summary>
    /// <param name="standardOutput">The standard output from the build process.</param>
    protected override void FormatOutput(string standardOutput)
    {
        Outputs = standardOutput.Split("\n").ToList();
    }

    /// <summary>
    /// Formats the error output by splitting it into lines and filtering for specific error codes.
    /// </summary>
    /// <param name="errorOutput">The error output from the build process.</param>
    protected override void FormatErrors(string errorOutput)
    {
        Errors = errorOutput.Split("\n").Where(x => x.Contains("CS") || x.Contains("IDE")).ToList();
    }
}