using GitHooksCsharp.Commit.Message;

namespace GitHooksCsharp.Hooks.CommitMsg;

/// <summary>
/// Represents the commit message hook.
/// </summary>
public static class CommitMsgHook
{
    /// <summary>
    /// Gets the list of tasks to be executed by the hook.
    /// </summary>
    public static List<BaseHookTask> Tasks { get; } = [];
    
    /// <summary>
    /// Executes the commit message hook.
    /// </summary>
    public static void Execute()
    {
        Configuration.Synchronize();
        
        string commitMsgFilepath = Environment.GetCommandLineArgs()[2];
        string[] commitMsg = File.ReadAllLines(commitMsgFilepath).Where(x => !x.StartsWith('#')).ToArray();

        HookLogger hookLogger = new(HookTypes.CommitMsg);
        hookLogger.StartHook();

        CommitMessage commitMessage = new(commitMsg);
        
        Tasks.Add(new CheckSubjectTask(hookLogger, commitMessage));
        Tasks.Add(new CheckBodyTask(hookLogger, commitMessage));
        Tasks.Add(new CheckBlankLinesTask(hookLogger, commitMessage));
        Tasks.Add(new CheckFooterTask(hookLogger, commitMessage));
        
        Tasks.ForEach(t => t.Execute());

        bool hookPassed = Tasks.TrueForAll(t => t.Success);
        
        hookLogger.FinishHook(hookPassed);
        Environment.Exit(hookPassed ? 0 : 1);
    }
}