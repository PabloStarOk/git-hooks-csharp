using GitHooksCsharp.Logging;

namespace GitHooksCsharp.Hooks;

// TODO: Implement ILogger interface.
/// <summary>
/// The HookLogger class provides logging functionalities for different hook types.
/// </summary>
public class HookLogger
{
    private readonly HookTypes _hookType;
    private readonly string _title;

    /// <summary>
    /// Initializes a new instance of the HookLogger class.
    /// </summary>
    /// <param name="hookType">The type of the hook.</param>
    public HookLogger(HookTypes hookType)
    {
        _hookType = hookType;
        _title = $"-------------- 🪝 {_hookType} 🕵🏻 --------------";
    }

    /// <summary>
    /// Logs the start of a hook.
    /// </summary>
    public void StartHook()
    {
        Logger.BlankLine();
        Logger.Log(ConsoleColor.Cyan, _title);
    }

    /// <summary>
    /// Logs that the hook was skipped and finishes the hook.
    /// </summary>
    public void SkipHook()
    {
        Logger.LogWarning($"⚠️ {_hookType}: Skipped ⚠️");
        FinishHook();
    }
    
    /// <summary>
    /// Logs the end of a hook.
    /// </summary>
    public void FinishHook()
    {
        Logger.Log(ConsoleColor.Cyan, _title);
        Logger.BlankLine();
    }
    
    /// <summary>
    /// Logs the result of the hook based on success or failure.
    /// </summary>
    /// <param name="success">Indicates whether the hook was successful.</param>
    public void FinishHook(bool success)
    {
        if (success)
            Logger.LogSuccess($"✅ {_hookType}: Checks passed ✅");
        else
            Logger.LogError($"❌ {_hookType}: Failed to pass the checks ❌");
        
        FinishHook();
    }
    
    /// <summary>
    /// Logs the start of a task.
    /// </summary>
    /// <param name="output">The output message to log.</param>
    public void StartTask(string output)
    {
        Logger.LogInfo($"➡️ {output}");
    }

    /// <summary>
    /// Logs the result of a task based on success or failure.
    /// </summary>
    /// <param name="output">The output message to log.</param>
    /// <param name="success">Indicates whether the task was successful.</param>
    public void FinishTask(string output, bool success)
    {
        if (success)
            Logger.LogSuccess($"\t✅ {output}");
        else
            Logger.LogError($"\t❌ {output}");
        
        Logger.BlankLine();
    } 
    
    /// <summary>
    /// Logs the result of a task based on success or failure, including additional result details.
    /// </summary>
    /// <typeparam name="T">The type of the result.</typeparam>
    /// <param name="output">The output message to log.</param>
    /// <param name="success">Indicates whether the task was successful.</param>
    /// <param name="result">The result object containing additional details.</param>
    public void FinishTask<T>(string output, bool success, BaseResult<T>? result = default) where T : class
    {
        if (success)
        {
            Logger.LogSuccess($"\t✅ {output}");
            
            if (result != default)
            {
                foreach (var error in result.Outputs)
                    Logger.LogError($"\t\t🟨 {error}");
            }
        }
        else
        {
            Logger.LogError($"\t❌ {output}");
            
            if (result != default)
            {
                foreach (var error in result.Errors)
                    Logger.LogError($"\t\t🟨 {error}");
            }
        }

        Logger.BlankLine();
    }
}