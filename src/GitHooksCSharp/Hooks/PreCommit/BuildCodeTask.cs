using GitHooksCsharp.DotnetCommands;

namespace GitHooksCsharp.Hooks.PreCommit;

/// <summary>
/// Represents a task that builds code for the specified project paths.
/// </summary>
/// <param name="hookLogger">The logger used to log task information.</param>
/// <param name="projectPaths">The paths of the projects to build.</param>
public class BuildCodeTask(HookLogger hookLogger, params string[] projectPaths) : BaseHookTask(hookLogger)
{
    private readonly string[] _projectPaths = projectPaths;
    
    /// <summary>
    /// Executes the build task. If there is only one project path, it builds it synchronously.
    /// If there are multiple project paths, it builds them asynchronously.
    /// </summary>
    public override void Execute()
    {
        _hookLogger.StartTask("Building code");
        
        if (_projectPaths.Length < 2)
        {
            BuildResult result = Dotnet.BuildCode();
            Success = result.ExitCode == 0;
            _hookLogger.FinishTask(Success ? "Build finished" : "Build failed",
                Success,
                Success ? default : result); // Don't log output when success
            return;
        }
        
        List<Task<BuildResult>> tasks = _projectPaths.Select(Dotnet.BuildCodeAsync).ToList();
        while (tasks.Count > 0)
        {
            var completedTask = Task.WhenAny(tasks).Result;
            tasks.Remove(completedTask);

            bool taskSuccess = completedTask.Result.ExitCode == 0;
            if (!taskSuccess)
                Success = false;

            string resultStr = taskSuccess ? "finished" : "failed";
            _hookLogger.FinishTask($"Build {resultStr} in project {completedTask.Result.Path}",
                taskSuccess,
                taskSuccess ? default : completedTask.Result); // Don't log output when success
        }
    }
}