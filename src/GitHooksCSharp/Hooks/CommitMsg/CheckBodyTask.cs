using System.Text.RegularExpressions;
using GitHooksCsharp.Commit.Message;
using GitHooksCsharp.Commit.Validation;
using GitHooksCsharp.Text;

namespace GitHooksCsharp.Hooks.CommitMsg;

/// <summary>
/// Task to check the body of a commit message.
/// </summary>
/// <param name="hookLogger">Logger for the hook tasks.</param>
/// <param name="commitMessage">The commit message to be checked.</param>
public class CheckBodyTask(HookLogger hookLogger, CommitMessage commitMessage) : BaseHookTask(hookLogger)
{
    private readonly CommitMessage _commitMessage = commitMessage;
    
    /// <summary>
    /// Executes the task to check the commit message body.
    /// </summary>
    public override void Execute()
    {
        bool isRequired = Configuration.CommitMessageRules.Body.IsRequired;
        if (isRequired && string.IsNullOrWhiteSpace(_commitMessage.Body))
        {
            Success = false;
            _hookLogger.StartTask($"Verifying the existence of the body");
            _hookLogger.FinishTask("The commit message must contain a body.", Success);
        }
        else if (!string.IsNullOrWhiteSpace(_commitMessage.Body))
        {
            _hookLogger.StartTask("Verifying commit message body format");

            Match match = Regex.Match(_commitMessage.Body, CommitMessageRegex.BodyRegex);
            Success = match.Success;
            
            CheckDescriptionLength();

            string resultStr = "Body of the commit message is " + (Success ? "correct." : "incorrect.");
            _hookLogger.FinishTask(resultStr, Success);
        }
    }
    
    // HACK: Verify max length to avoid issues when using title case.
    // TODO: Fix HACK or at least refactor this duplicated method.
    /// <summary>
    /// Checks the length of the description in the commit message body.
    /// </summary>
    private void CheckDescriptionLength()
    {
        if (Configuration.CommitMessageRules.Body.LetterCase != LetterCase.Title)
            return;
        
        int maxLength = Configuration.CommitMessageRules.Body.MaxLength;
        if (!Success || maxLength <= 0)
            return;

        // Find description index
        Match divisorMatch = Regex.Match(_commitMessage.Body, @":\s");
        int descriptionIndex = divisorMatch.Index + 2;

        string description = string.Empty;
        if (descriptionIndex < _commitMessage.Body.Length)
            description = _commitMessage.Body[descriptionIndex.._commitMessage.Body.Length];

        _hookLogger.StartTask("Verifying description length");
        string resultStr = "Description length is " + (description.Length <= maxLength ? "meet." : "not meet.");
        _hookLogger.FinishTask(resultStr, description.Length <= maxLength);
        
        Success = description.Length <= maxLength;
    }
}