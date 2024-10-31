using System.Text.RegularExpressions;
using GitHooksCsharp.Commit.Message;
using GitHooksCsharp.Commit.Validation;
using GitHooksCsharp.Text;

namespace GitHooksCsharp.Hooks.CommitMsg;

/// <summary>
/// Task to check the subject of a commit message.
/// </summary>
/// <param name="hookLogger">Logger for the hook tasks.</param>
/// <param name="commitMessage">The commit message to be checked.</param>
public class CheckSubjectTask(HookLogger hookLogger, CommitMessage commitMessage) : BaseHookTask(hookLogger)
{
    private readonly CommitMessage _commitMessage = commitMessage;
    
    /// <summary>
    /// Executes the task to verify the commit message subject format.
    /// </summary>
    public override void Execute()
    {
        _hookLogger.StartTask("Verifying commit message subject format");
        
        Match match = Regex.Match(_commitMessage.Subject, CommitMessageRegex.SubjectRegex);
        Success = match.Success;

        CheckDescriptionLength();
        
        string resultStr = "Subject of the commit message is " + (Success ? "correct." : "incorrect.");
        _hookLogger.FinishTask(resultStr, Success);
    }

    // HACK: Verify max length to avoid issues when using title case.
    /// <summary>
    /// Checks the length of the description in the commit message subject.
    /// </summary>
    private void CheckDescriptionLength()
    {
        if (Configuration.CommitMessageRules.Subject.DescriptionRules.LetterCase != LetterCase.Title)
            return;
        
        int maxLength = Configuration.CommitMessageRules.Subject.DescriptionRules.MaxLength;
        if (!Success || maxLength <= 0)
            return;

        // Find description index
        Match divisorMatch = Regex.Match(_commitMessage.Subject, @":\s");
        int descriptionIndex = divisorMatch.Index + 2;

        string description = string.Empty;
        if (descriptionIndex < _commitMessage.Subject.Length)
            description = _commitMessage.Subject[descriptionIndex.._commitMessage.Subject.Length];

        _hookLogger.StartTask("Verifying description length");
        string resultStr = "Description length is " + (description.Length <= maxLength ? "meet." : "not meet.");
        _hookLogger.FinishTask(resultStr, description.Length <= maxLength);
        Success = description.Length <= maxLength;
    }
}