namespace GitHooksCsharp.Hooks.PreCommit;

/// <summary>
/// Represents the pre-commit hook.
/// </summary>
public static class PreCommitHook
{
    /// <summary>
    /// Tasks to execute before committing.
    /// </summary>
    public static List<BaseHookTask> Tasks { get; } = [];
    
    /// <summary>
    /// Projects paths to format, clean, build and test.
    /// </summary>
    public static List<string> ProjectPaths { get; } = [];
    
    /// <summary>
    /// Test projects paths to test.
    /// </summary>
    public static List<string> TestProjectPaths { get; } = [];
    
    /// <summary>
    /// Executes the pre-commit hook tasks.
    /// Formats, cleans, builds, and tests the project.
    /// </summary>
    public static void Execute()
    {
        // Synchronize configuration settings
        Configuration.Synchronize();
        
        // Initialize the hook logger for the pre-commit hook
        HookLogger hookLogger = new(HookTypes.PreCommit);
        hookLogger.StartHook();

        // Add tasks to the task list
        Tasks.Add(new FormatCodeTask(hookLogger, ProjectPaths.ToArray()));
        Tasks.Add(new CleanTask(hookLogger, ProjectPaths.ToArray()));
        Tasks.Add(new BuildCodeTask(hookLogger, ProjectPaths.ToArray()));
        Tasks.Add(new TestCodeTask(hookLogger, TestProjectPaths.ToArray()));
        
        // Execute all tasks
        Tasks.ForEach(t => t.Execute());

        // Determine if all tasks were successful
        bool hookPassed = Tasks.TrueForAll(t => t.Success);

        // Finish the hook and exit with the appropriate status code
        hookLogger.FinishHook(hookPassed);
        Environment.Exit(hookPassed ? 0 : 1);
    }
}