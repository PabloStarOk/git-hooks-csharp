using System.Text.Json.Serialization;
using GitHooksCsharp.Text;

namespace GitHooksCsharp.Commit.Message;

/// <summary>
/// Represents the rules for a commit message.
/// </summary>
public class CommitMessageRules
{
    /// <summary>
    /// Gets the rules for the subject of the commit message.
    /// </summary>
    public SubjectRules Subject { get; }

    /// <summary>
    /// Gets the rules for the body of the commit message.
    /// </summary>
    public CommitMessageComponentRules Body { get; }

    /// <summary>
    /// Gets the rules for the footer of the commit message.
    /// </summary>
    public FooterRules Footer { get; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CommitMessageRules"/> class.
    /// </summary>
    public CommitMessageRules()
    {
        Subject = new SubjectRules();
        Body = new CommitMessageComponentRules(false, LetterCase.Any, 0, 0);
        Footer = new FooterRules();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommitMessageRules"/> class.
    /// </summary>
    /// <param name="subject">The rules for the subject of the commit message.</param>
    /// <param name="body">The rules for the body of the commit message.</param>
    /// <param name="footer">The rules for the footer of the commit message.</param>
    [JsonConstructor]
    public CommitMessageRules(SubjectRules? subject, CommitMessageComponentRules? body, FooterRules? footer)
    {
        Subject = subject ?? new SubjectRules();
        Body = body ?? new CommitMessageComponentRules(false, LetterCase.Any, 0, 0);
        Footer = footer ?? new FooterRules();
    }
}






