using System.Text.RegularExpressions;
using GitHooksCsharp.Commit.Message;
using GitHooksCsharp.Commit.Validation;
using GitHooksCsharp.Text;

namespace GitHooksCsharp.Hooks.CommitMsg;

/// <summary>
/// Task to check the footer of a commit message.
/// </summary>
/// <param name="hookLogger">Logger for the hook tasks.</param>
/// <param name="commitMessage">The commit message to be checked.</param>
public class CheckFooterTask(HookLogger hookLogger, CommitMessage commitMessage) : BaseHookTask(hookLogger)
{
    private readonly CommitMessage _commitMessage = commitMessage;
    
    /// <summary>
    /// Executes the task to check the footer of the commit message.
    /// </summary>
    public override void Execute()
    {
        bool isRequired = Configuration.CommitMessageRules.Footer.IsRequired;
        if (isRequired && _commitMessage.Footers.Count <= 0)
        {
            Success = false;
            _hookLogger.StartTask($"Verifying the existence of a footer");
            _hookLogger.FinishTask("There must be at least one footer in the commit message.", Success);
            return;
        }

        string[] footers = _commitMessage.Footers.Where(f => !string.IsNullOrWhiteSpace(f)).ToArray();
        foreach (var footer in footers)
        {
            _hookLogger.StartTask($"Verifying the format of the footer: \"{GetStrTruncated(footer)}\"");

            Match match = Regex.Match(footer, CommitMessageRegex.FooterRegex);

            bool checksResult = match.Success && CheckDescriptionLength(footer, match.Success);
            
            string resultStr = "Footer is " + (checksResult ? "correct." : "incorrect.");
            _hookLogger.FinishTask(resultStr, checksResult);
            
            if (!match.Success)
                Success = false;
        }
    }
    
    // HACK: Verify max length to avoid issues when using title case.
    // TODO: Fix HACK or at least refactor this duplicated method.
    /// <summary>
    /// Checks the length of the description in the footer.
    /// </summary>
    /// <param name="footer">The footer to be checked.</param>
    /// <param name="matchSuccess">Indicates if the footer matched the regex.</param>
    /// <returns>True if the description length is within the allowed limit, otherwise false.</returns>
    private bool CheckDescriptionLength(string footer, bool matchSuccess)
    {
        if (Configuration.CommitMessageRules.Footer.DescriptionRules.LetterCase != LetterCase.Title)
            return true;
        
        int maxLength = Configuration.CommitMessageRules.Footer.DescriptionRules.MaxLength;
        if (!matchSuccess && maxLength <= 0)
            return true;
        
        // Find description index
        Match divisorMatch = Regex.Match(footer, @":\s");
        int descriptionIndex = divisorMatch.Index + 2;

        string description = string.Empty;
        if (descriptionIndex < footer.Length)
            description = footer[descriptionIndex..footer.Length];
        
        string resultStr = "Description length is " + (description.Length <= maxLength ? "meet." : "not meet.");
        _hookLogger.FinishTask(resultStr, description.Length <= maxLength);
        
        return description.Length <= maxLength;
    }
    
    /// <summary>
    /// Truncates the given string to a specified maximum length.
    /// </summary>
    /// <param name="str">The string to be truncated.</param>
    /// <param name="maxLength">The maximum length of the truncated string.</param>
    /// <returns>The truncated string.</returns>
    private static string GetStrTruncated(string str, int maxLength = 50)
    {
        return str.Length <= maxLength ? str : string.Concat(str.AsSpan(0, maxLength - 3), "...");
    }
}