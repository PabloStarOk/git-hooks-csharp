using System.Text.Json;
using System.Text.Json.Serialization;
using GitHooksCsharp.Commit.Message;
using GitHooksCsharp.Logging;

namespace GitHooksCsharp;

/// <summary>
/// Provides configuration management for Git hooks.
/// </summary>
public static class Configuration
{
    // TODO: Allow to set the target projects and test projects to be used in the configuration file.
    // TODO: Allow to provide custom config paths and change the name.
    private const string ConfigFileName = "cshooks-config.json";
    private readonly static JsonSerializerOptions JsonSerializerOptions = new()
    {
        DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        PropertyNameCaseInsensitive = true,
        Converters =
        {
            new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
        }
    };
    public static CommitMessageRules CommitMessageRules { get; private set; } = new();

    /// <summary>
    /// Synchronizes the configuration by reading from or creating the configuration file.
    /// </summary>
    public static void Synchronize()
    {
        // TODO: Save configuration where library is.
        string configFolder = Path.Combine(Environment.CurrentDirectory, ".git/hooks");
        string path;
        if (Directory.Exists(configFolder))
            path = Path.Combine(configFolder, ConfigFileName);
        else
        {
            Logger.LogWarning(".git/hooks folder not found. Using current environment directory.");
            path = Path.Combine(Environment.CurrentDirectory, ConfigFileName);
        }
        
        if (!File.Exists(path))
        {
            CreateConfig(path);
            return;
        }
        
        ReadConfig(path);
    }

    /// <summary>
    /// Creates the configuration file with default settings.
    /// </summary>
    /// <param name="path">The path where the configuration file will be created.</param>
    private static void CreateConfig(string path)
    {
        try
        {
            using FileStream fileStream = File.OpenWrite(path);
            JsonSerializer.Serialize(fileStream, CommitMessageRules, JsonSerializerOptions);

            Logger.LogSuccess($"Configuration created in {path}. Using default configuration.");
        }
        catch(Exception ex)
        {
            Logger.LogError($"Error while creating configuration: {ex.Message}");
        }
    }

    /// <summary>
    /// Reads the configuration from the specified file.
    /// </summary>
    /// <param name="path">The path of the configuration file to read.</param>
    private static void ReadConfig(string path)
    {
        try
        {
            using FileStream fileStream = File.OpenRead(path);
                CommitMessageRules? msgRules = JsonSerializer.Deserialize<CommitMessageRules>(fileStream, JsonSerializerOptions);

            if (msgRules != null)
            {
                CommitMessageRules = msgRules;
                Logger.LogSuccess($"Configuration read from {path}.");
            }
            else
                Logger.LogError("Configuration could not be read from {path}.");
        }
        catch (Exception ex)
        {
            Logger.LogError($"Error while reading configuration: {ex.Message}");
        }
    }
}