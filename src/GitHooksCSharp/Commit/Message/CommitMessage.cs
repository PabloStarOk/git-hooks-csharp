using GitHooksCsharp.Logging;
using System.Text.RegularExpressions;
using GitHooksCsharp.Commit.Validation;

namespace GitHooksCsharp.Commit.Message;

/// <summary>
/// Represents a commit message providing its parts and ways to validate them.
/// </summary>
public class CommitMessage
{
    /// <summary>
    /// Gets the raw commit message as an array of strings.
    /// </summary>
    public string[] RawMessage { get; }

    /// <summary>
    /// Gets the subject of the commit message.
    /// </summary>
    public string Subject { get; private set; }
    
    /// <summary>
    /// Gets the body of the commit message.
    /// </summary>
    public string Body { get; private set; } = string.Empty;
    
    /// <summary>
    /// Gets the list of footers in the commit message.
    /// </summary>
    public List<string> Footers { get; private set; } = [];

    /// <summary>
    /// Indicates whether there is a blank line after the subject.
    /// </summary>
    public bool BlankLineAfterSubject { get; private set; }
    
    /// <summary>
    /// Indicates whether there is a blank line after the body.
    /// </summary>
    public bool BlankLineAfterBody { get; private set; }
    
    /// <summary>
    /// Gets the breaking change information from the commit message.
    /// </summary>
    public string BreakingChange { get; private set; } = string.Empty;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommitMessage"/> class with the specified raw message.
    /// </summary>
    /// <param name="message">The raw commit message as an array of strings.</param>
    public CommitMessage(string[] message)
    {
        RawMessage = message;

        Subject = RawMessage[0];

        switch (RawMessage.Length)
        {
            // Save the rest of the message
            case > 2 when RawMessage[1] == string.Empty:
                BlankLineAfterSubject = true;
                SeparateBodyAndFooters(RawMessage[2..]);
                break;
            case > 1:
                SeparateBodyAndFooters(RawMessage[1..]);
                break;
        }

        SaveBreakingChange();

        LogMessage("Subject", Subject);
        LogMessage("Body", Body);
        LogMessage<ICollection<string>>("Footers", Footers);
        LogMessage("Breaking Change", BreakingChange);
    }

    /// <summary>
    /// Separates the body and footers from the commit message.
    /// </summary>
    /// <param name="commitMessage">The commit message as an array of strings.</param>
    private void SeparateBodyAndFooters(string[] commitMessage)
    {
        if (commitMessage.Length == 0)
            return;

        int firstFooterIndex = commitMessage.Length;

        // Find index first footer
        foreach (var line in commitMessage)
        {
            Match match = Regex.Match(line, CommitMessageRegex.FooterKeywordRegex);
            if (!match.Success)
                continue;
            
            firstFooterIndex = Array.IndexOf(commitMessage, line);
            break;
        }

        // Save body lines
        List<string> bodyLines = commitMessage[..firstFooterIndex].ToList();
        FormatBody(bodyLines);

        // Save the rest of the footers
        string[] footerLines = commitMessage[firstFooterIndex..commitMessage.Length];
        FormatFooters(footerLines);
    }

    /// <summary>
    /// Formats the body of the commit message.
    /// </summary>
    /// <param name="bodyLines">The body lines as a list of strings.</param>
    private void FormatBody(List<string> bodyLines)
    {
        if (bodyLines.Count == 0)
            return;

        if (bodyLines[^1] == string.Empty)
        {
            bodyLines.RemoveAt(bodyLines.Count - 1);
            BlankLineAfterBody = true;
        }

        Body = string.Join("\n", bodyLines);
    }

    /// <summary>
    /// Formats the footers of the commit message.
    /// </summary>
    /// <param name="footerLines">The footer lines as an array of strings.</param>
    private void FormatFooters(string[] footerLines)
    {
        if (footerLines.Length == 0)
            return;

        foreach (string line in footerLines)
        {
            if (line == string.Empty)
                continue;
            
            // Check if it's intended to be a footer.
            Match match = Regex.Match(line, CommitMessageRegex.FooterKeywordRegex);

            if (match.Success)
                Footers.Add(line);
            else
            {
                int lastFooter = Footers.Count - 1;
                if (lastFooter >= 0)
                    Footers[lastFooter] += '\n' + line.Trim().TrimEnd('\n');
            }
        }
    }

    /// <summary>
    /// Saves the breaking change information from the footers.
    /// </summary>
    private void SaveBreakingChange()
    {
        static bool Predicate(string f) => f.Contains("BREAKING CHANGE") || f.Contains("BREAKING-CHANGE");

        if (Footers.Exists(Predicate))
            BreakingChange = Footers.First(Predicate);
    }

    /// <summary>
    /// Logs the specified part of the commit message.
    /// </summary>
    /// <typeparam name="T">The type of the message part.</typeparam>
    /// <param name="partName">The name of the message part.</param>
    /// <param name="message">The message part to log.</param>
    private static void LogMessage<T>(string partName, T message)
    {
        if (message is string messageStr)
        {
            Logger.BlankLine();
            Logger.Log(ConsoleColor.Blue, $"\t🟦 {partName}:");
            Logger.LogInfo(messageStr != string.Empty ? $"\t- '{messageStr}'" : $"\t- Empty.");
        }
        else if (message is ICollection<string> messageCollection)
        {
            Logger.BlankLine();
            Logger.Log(ConsoleColor.Blue, $"\t🟦 {partName}:");
            if (messageCollection.Count > 0)
            {
                foreach (var line in messageCollection)
                    Logger.LogInfo($"\t- '{line}'");
            }
            else
                Logger.LogInfo($"\t- Empty.");
        }
        Logger.BlankLine();
    }
}