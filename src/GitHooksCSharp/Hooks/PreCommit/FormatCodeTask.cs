using GitHooksCsharp.Cmd;
using GitHooksCsharp.DotnetCommands;

namespace GitHooksCsharp.Hooks.PreCommit;

/// <summary>
/// Represents a task that formats code in the specified project paths before a commit.
/// </summary>
/// <param name="hookLogger">The logger used to log the task's progress and results.</param>
/// <param name="projectPaths">The paths of the projects to format.</param>
public class FormatCodeTask(HookLogger hookLogger, params string[] projectPaths) : BaseHookTask(hookLogger)
{
    private readonly string[] _projectPaths = projectPaths;
    
 
    /// <summary>
    /// Executes the format code task. If there is only one project path, it runs synchronously.
    /// Otherwise, it runs the format command asynchronously for each project path.
    /// </summary>
    public override void Execute()
    {
        _hookLogger.StartTask("Formatting code");
        
        if (_projectPaths.Length < 2)
        {
            CmdResult result = Dotnet.FormatCode();
            Success = result.ExitCode == 0;
            _hookLogger.FinishTask(Success ? "Format finished" : "Format failed",
                Success,
                Success ? default : result); // Don't log output when success
            return;
        }
        
        List<Task<CmdResult>> tasks = _projectPaths.Select(Dotnet.FormatCodeAsync).ToList();
        while (tasks.Count > 0)
        {
            var completedTask = Task.WhenAny(tasks).Result;
            tasks.Remove(completedTask);

            bool taskSuccess = completedTask.Result.ExitCode == 0;
            if (!taskSuccess)
                Success = false;

            string resultStr = taskSuccess ? "finished" : "failed";
            _hookLogger.FinishTask($"Format {resultStr} in project {completedTask.Result.Path}",
                taskSuccess,
                taskSuccess ? default : completedTask.Result); // Don't log output when success
        }
    }
}