namespace GitHooksCsharp;

/// <summary>
/// Represents a base result class for handling standard output, error output, and exit codes of
/// command line executions.
/// </summary>
/// <typeparam name="T">The type of the output and error items, which must be a class.</typeparam>
public abstract class BaseResult<T> where T : class
{
    /// <summary>
    /// Gets the standard output of the result.
    /// </summary>
    public string StandardOutput { get; protected set; }

    /// <summary>
    /// Gets the error output of the result.
    /// </summary>
    public string ErrorOutput { get; protected set; }

    /// <summary>
    /// Gets the exit code of the result.
    /// </summary>
    public int ExitCode { get; protected set; }

    /// <summary>
    /// Gets the associated path of the result.
    /// </summary>
    public string Path { get; protected set; }

    /// <summary>
    /// Gets the list of output items.
    /// </summary>
    public List<T> Outputs { get; protected set; } = [];

    /// <summary>
    /// Gets the list of error items.
    /// </summary>
    public List<T> Errors { get; protected set; } = [];

    /// <summary>
    /// Initializes a new instance of the <see cref="BaseResult{T}"/> class.
    /// </summary>
    /// <param name="standardOutput">The standard output of the result.</param>
    /// <param name="exitCode">The exit code of the result.</param>
    /// <param name="errorOutput">The error output of the result. Default is an empty string.</param>
    /// <param name="path">Associated path of the result.</param>
    protected BaseResult(string standardOutput, int exitCode, string errorOutput = "", string path = "")
    {
        StandardOutput = standardOutput;
        ExitCode = exitCode;
        ErrorOutput = errorOutput;
        Path = path;
        
        Initialize();
    }
    
    /// <summary>
    /// Initializes the result by formatting the output and errors.
    /// </summary>
    private void Initialize()
    {
        FormatOutput(StandardOutput);
        FormatErrors(ErrorOutput);
    }

    /// <summary>
    /// Formats the standard output.
    /// </summary>
    /// <param name="standardOutput">The standard output to format.</param>
    protected abstract void FormatOutput(string standardOutput);

    /// <summary>
    /// Formats the error output.
    /// </summary>
    /// <param name="errorOutput">The error output to format.</param>
    protected abstract void FormatErrors(string errorOutput);
}