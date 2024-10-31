using GitHooksCsharp.Utils;

namespace GitHooksCsharp.Hooks.PrepareCommitMsg;

/// <summary>
/// Represents the hook for preparing the commit message.
/// </summary>
public static class PrepareCommitMsgHook
{
    /// <summary>
    /// Tasks to execute when preparing the commit message.
    /// </summary>
    public static List<BaseHookTask> Tasks { get; } = [];
    
    /// <summary>
    /// Executes the prepare commit message hook.
    /// </summary>
    public static void Execute()
    {
        // TODO: Indexes of the arguments can be different.
        string[] args = Environment.GetCommandLineArgs();
        string commitMessageFilePath = CmdUtils.GetCommandLineArg(args, 2);
        string commitType = CmdUtils.GetCommandLineArg(args, 3);

        // Exit if the user already gave a message.
        if (commitType.Equals("message"))
            Environment.Exit(0);
        
        // Synchronize the configuration.
        Configuration.Synchronize();
        
        // Initialize the hook logger for the PrepareCommitMsg hook.
        HookLogger hookLogger = new(HookTypes.PrepareCommitMsg);
        hookLogger.StartHook();
        
        // Add the MessageTemplateTask to the list of tasks and execute all tasks.
        Tasks.Add(new MessageTemplateTask(hookLogger, commitMessageFilePath));
        Tasks.ForEach(t => t.Execute());
        
        // Check if all tasks passed successfully.
        bool hookPassed = Tasks.TrueForAll(t => t.Success);

        // Finish the hook and exit with the appropriate status code.
        hookLogger.FinishHook(hookPassed);
        Environment.Exit(hookPassed ? 0 : 1);
    }
}
