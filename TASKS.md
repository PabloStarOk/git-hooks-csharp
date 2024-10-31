## Tasks

### Important
- [ ] Check TODOs in the code.
- [ ] Add tests.
- [ ] Add `RegexTextGenerator` to add regex restrictions dynamically using methods (e.g., `TextRegexGenerator.AddMinLength(...)`) or use a NuGet library.
- [ ] Add log lines with multicolor.
- [ ] Allow to specify the Command Line process to execute commands [CommandLine.cs](src/GitHooksCSharp/Cmd/CommandLine.cs)
- [ ] Allow to set the target projects and test projects to be used in the configuration file [Configuration.cs](src/GitHooksCSharp/Configuration.cs)
- [ ] Allow to provide custom config paths and change the name [Configuration.cs](src/GitHooksCSharp/Configuration.cs)
- [ ] Allow to use parameters for the commands. [DotNetCommand.cs](src/GitHooksCSharp/DotnetCommands/Dotnet.cs)
- [ ] Implement ILogger interface for loggers [HookLogger.cs](src/GitHooksCSharp/Hooks/HookLogger.cs) [Logger.cs](src/GitHooksCSharp/Logging/Logger.cs) and DI.
- [ ] Indices of the arguments can be different, fix it. [PrepareCommitMsgHook.cs](src/GitHooksCSharp/Hooks/PrepareCommitMsg/PrepareCommitMsgHook.cs)
- [ ] Fix HACK or at least refactor this duplicated method. [CheckSubjectTask.cs](src/GitHooksCSharp/Hooks/CommitMsg/CheckSubjectTask.cs),
[CheckBodyTask.cs](src/GitHooksCSharp/Hooks/CommitMsg/CheckBodyTask.cs) and [CheckFooterTask.cs](src/GitHooksCSharp/Hooks/CommitMsg/CheckFooterTask.cs)

### Hooks
- [ ] Add hook execution config to specify which hook scripts must be executed.
- [ ] Add an automatic message formatter.
- [ ] Add more descriptive error messages to indicate what is wrong with the commit message.
- [ ] Add a pre-push hook.

### Configuration File
- [ ] Add a property to set a limit on the number of footers.
- [ ] Add a whitespace check (body or descriptions with only whitespaces).
- [ ] Add disallowed keywords for Subject and Footers.

