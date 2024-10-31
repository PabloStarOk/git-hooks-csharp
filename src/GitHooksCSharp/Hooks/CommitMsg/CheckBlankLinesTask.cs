using GitHooksCsharp.Commit.Message;

namespace GitHooksCsharp.Hooks.CommitMsg;

/// <summary>
/// Task to check for blank lines in a commit message.
/// </summary>
/// <param name="hookLogger">Logger for the hook tasks.</param>
/// <param name="commitMessage">The commit message to be checked.</param>
public class CheckBlankLinesTask(HookLogger hookLogger, CommitMessage commitMessage) : BaseHookTask(hookLogger)
{
    private readonly CommitMessage _commitMessage = commitMessage;
    
    /// <summary>
    /// Executes the task to check for blank lines after the subject and body of the commit message.
    /// </summary>
    public override void Execute()
    {
        bool afterSubject = AfterSubject();
        bool afterBody = AfterBody();
        Success = afterSubject && afterBody;
    }
    
    /// <summary>
    /// Checks if there is a blank line after the subject of the commit message.
    /// </summary>
    /// <returns>True if there is a blank line after the subject, otherwise false.</returns>
    private bool AfterSubject()
    {
        if (_commitMessage is { Body: "", Footers.Count: 0 })
            return true;

        _hookLogger.StartTask($"Verifying blank line after subject");
        string resultStr = $"Blank line after subject is " + (_commitMessage.BlankLineAfterSubject ? "present." : "missing.");
        _hookLogger.FinishTask(resultStr, _commitMessage.BlankLineAfterSubject);

        return _commitMessage.BlankLineAfterSubject;
    }

    /// <summary>
    /// Checks if there is a blank line after the body of the commit message.
    /// </summary>
    /// <returns>True if there is a blank line after the body, otherwise false.</returns>
    private bool AfterBody()
    {
        if (_commitMessage.Body == string.Empty || _commitMessage.Footers.Count == 0)
            return true;

        _hookLogger.StartTask($"Verifying blank line after body");
        string resultStr = $"Blank line after body is " + (_commitMessage.BlankLineAfterBody ? "present." : "missing.");
        _hookLogger.FinishTask(resultStr, _commitMessage.BlankLineAfterBody);

        return _commitMessage.BlankLineAfterBody;
    }
}