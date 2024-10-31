using GitHooksCsharp.DotnetCommands;

namespace GitHooksCsharp.Hooks.PreCommit;

/// <summary>
/// Represents a task that runs tests on the specified projects before a commit.
/// </summary>
/// <param name="hookLogger">The logger used to log the task's progress and results.</param>
/// <param name="testProjectPaths">The paths to the test projects to be tested.</param>
public class TestCodeTask(HookLogger hookLogger, params string[] testProjectPaths) : BaseHookTask(hookLogger)
{
    private readonly string[] _testProjectPaths = testProjectPaths;
    
    /// <summary>
    /// Executes the test code task. If there is only one test project, it runs the tests synchronously.
    /// If there are multiple test projects, it runs the tests asynchronously.
    /// </summary>
    public override void Execute()
    {
        _hookLogger.StartTask("Testing code");
        
        if (_testProjectPaths.Length < 2)
        {
            TestsResult result = Dotnet.TestCode();
            Success = result.ExitCode == 0;
            _hookLogger.FinishTask(Success ? "Tests passed" : "Tests failed",
                Success,
                Success ? default : result); // Don't log output when success
            return;
        }
        
        List<Task<TestsResult>> tasks = _testProjectPaths.Select(Dotnet.TestCodeAsync).ToList();
        while (tasks.Count > 0)
        {
            var completedTask = Task.WhenAny(tasks).Result;
            tasks.Remove(completedTask);

            bool taskSuccess = completedTask.Result.ExitCode == 0;
            if (!taskSuccess)
                Success = false;

            string resultStr = taskSuccess ? "passed" : "failed";
            _hookLogger.FinishTask($"Tests {resultStr} in project {completedTask.Result.Path}",
                taskSuccess,
                taskSuccess ? default : completedTask.Result); // Don't log output when success
        }
    }
}