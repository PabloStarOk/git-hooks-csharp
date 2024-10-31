namespace GitHooksCsharp.Hooks;

/// <summary>
/// Represents the base class for hook tasks.
/// </summary>
/// <param name="hookLogger">The logger used for logging hook execution details.</param>
public abstract class BaseHookTask(HookLogger hookLogger)
{
    protected readonly HookLogger _hookLogger = hookLogger;

    /// <summary>
    /// Gets a value indicating whether the task was successful.
    /// </summary>
    public bool Success { get; protected set; } = true;

    /// <summary>
    /// Executes the hook task.
    /// </summary>
    public abstract void Execute();
}