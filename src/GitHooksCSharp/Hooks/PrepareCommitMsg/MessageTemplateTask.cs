using System.Text;
using GitHooksCsharp.Commit.Validation;

namespace GitHooksCsharp.Hooks.PrepareCommitMsg;

/// <summary>
/// Represents a task that prepares a commit message template.
/// </summary>
/// <param name="hookLogger">The logger used for logging hook tasks.</param>
/// <param name="messageFilePath">The file path where the commit message is stored.</param>
public class MessageTemplateTask(HookLogger hookLogger, string messageFilePath) : BaseHookTask(hookLogger)
{
    private readonly string _messageFilePath = messageFilePath;
    private string _template = string.Empty;
    
    /// <summary>
    /// Executes the task to prepare the commit message template.
    /// </summary>
    public override void Execute()
    {
        _hookLogger.StartTask("Preparing commit message template");

        List<string> msgTemplate =
        [
            "# Commit template based on: https://www.conventionalcommits.org/en/v1.0.0/",
            CommitMessageRegex.SubjectTemplate + Environment.NewLine,
            "# Optional body.",
            CommitMessageRegex.BodyTemplate + Environment.NewLine,
            "# Optional footers, as many as you want.",
            CommitMessageRegex.FooterTemplate,
            CommitMessageRegex.FooterTemplate
        ];

        _template = string.Join(Environment.NewLine, msgTemplate);
        WriteTemplate();
        _hookLogger.FinishTask("Commit message template prepared", true);
    }
    
    /// <summary>
    /// Writes the prepared template to the commit message file.
    /// </summary>
    private void WriteTemplate()
    {
        using MemoryStream ms = new(512);
        using BinaryWriter bw = new(ms, Encoding.UTF8);

        bw.Write(Encoding.UTF8.GetBytes($"{_template}{Environment.NewLine}"));

        using (FileStream fileStream = new(_messageFilePath, FileMode.Open, FileAccess.Read))
        using (StreamReader reader = new(fileStream, Encoding.UTF8))
            bw.Write(Encoding.UTF8.GetBytes(reader.ReadToEnd()));

        File.WriteAllBytes(_messageFilePath, ms.ToArray());
    }
}