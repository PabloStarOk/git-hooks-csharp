using GitHooksCsharp.Cmd;
using GitHooksCsharp.DotnetCommands;

namespace GitHooksCsharp.Hooks.PreCommit;

/// <summary>
/// Represents a task that cleans code in specified project paths before a commit.
/// </summary>
/// <param name="hookLogger">The logger used to log task progress and results.</param>
/// <param name="projectPaths">The paths of the projects to clean.</param>
public class CleanTask(HookLogger hookLogger, params string[] projectPaths) : BaseHookTask(hookLogger)
{
    private readonly string[] _projectPaths = projectPaths;
    
    /// <summary>
    /// Executes the clean task. If there is only one project path, it runs synchronously.
    /// Otherwise, it runs the clean command asynchronously for each project path.
    /// </summary>
    public override void Execute()
    {
        _hookLogger.StartTask("Cleaning code");
        
        if (_projectPaths.Length < 2)
        {
            CmdResult result = Dotnet.CleanCode();
            Success = result.ExitCode == 0;
            _hookLogger.FinishTask(Success ? "Clean finished" : "Clean failed",
                Success,
                Success ? default : result); // Don't log output when success
            return;
        }
        
        List<Task<CmdResult>> tasks = _projectPaths.Select(Dotnet.CleanCodeAsync).ToList();
        while (tasks.Count > 0)
        {
            var completedTask = Task.WhenAny(tasks).Result;
            tasks.Remove(completedTask);
            
            bool taskSuccess = completedTask.Result.ExitCode == 0;
            if (!taskSuccess)
                Success = false;

            string resultStr = taskSuccess ? "finished" : "failed";
            _hookLogger.FinishTask($"Clean {resultStr} in project {completedTask.Result.Path}",
                taskSuccess,
                taskSuccess ? default : completedTask.Result); // Don't log output when success
        }
    }
}