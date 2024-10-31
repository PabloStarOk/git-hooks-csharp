using GitHooksCsharp.Cmd;

namespace GitHooksCsharp.GitCommands;

/// <summary>
/// Provides static methods to execute common Git commands.
/// </summary>
public static class Git
{
    /// <summary>
    /// Stashes the current changes while keeping the index.
    /// </summary>
    public static void StashChanges()
    {
        CommandLine.Execute("git stash -q --keep-index");
    }

    /// <summary>
    /// Unstashes the most recently stashed changes.
    /// </summary>
    public static void UnstashChanges()
    {
        CommandLine.Execute("git stash pop -q");
    }

    /// <summary>
    /// Gets the name of the current Git branch.
    /// </summary>
    /// <returns>A <see cref="CmdResult"/> containing the name of the current branch.</returns>
    public static CmdResult CurrentBranch()
    {
        return CommandLine.Execute("git branch --show-current");
    }

    /// <summary>
    /// Gets the list of changed files in the working directory.
    /// </summary>
    /// <returns>A <see cref="CmdResult"/> containing the list of changed files.</returns>
    public static CmdResult ChangedFiles()
    {
        return CommandLine.Execute("git status -s");
    }
}
