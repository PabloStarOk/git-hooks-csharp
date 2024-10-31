# git-hooks-csharp

git-hooks-csharp is a library enabling developers to execute C#
code within Git hooks, offering automated checks to maintain
code quality and consistency in Git workflows. Currently, it provides three
hooks: `pre-commit`, `prepare-commit-msg`, and `commit-msg`.

## Table of Contents
1. [Hook Overview](#hook-overview)
2. [Key Features](#key-features)
3. [Usage](#usage)
4. [License](#license)
5. [Acknowledgments](#acknowledgments)

## Hook Overview
- [`PreCommit.cs`](src/GitHooksCSharp/Hooks/PreCommit/PreCommitHook.cs): Automates common dotnet commands for code formatting, cleaning, building, and testing.
- [`PrepareCommitMsg.cs`](src/GitHooksCSharp/Hooks/PrepareCommitMsg/PrepareCommitMsgHook.cs): Generates a basic commit message template, making it easier to create consistent commit messages.
- [`CommitMsg.cs`](src/GitHooksCSharp/Hooks/CommitMsg/CommitMsgHook.cs): Validates commit message format based on the
[Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/).

> [!NOTE]
> Part of this library is based on the Medium article [Using C# code in your git hooks](https://kaylumah.medium.com/using-c-code-in-your-git-hooks-66e507c01a0f).

## Key Features

### pre-commit Hook
- Format code using `dotnet format`
- Clean code using `dotnet clean`
- Build projects using `dotnet build`
- Run tests using `dotnet test`

### prepare-commit-msg Hook
- Prepares a basic commit message template when using `git commit` without the `-m` flag.

### commit-msg Hook
- Commit message format validation.
- Configuration in a JSON format file to modify the
constraints of the commit message format.

## Usage

1. You must install the `dotnet-script` tool:
    ```bash
    dotnet tool install -g dotnet-script
    ```

2. Compile this project and add the compiled DLL to the git hooks folder or wherever you need it.
3. Create a git hook to execute the DLL. The git hook file must be a dotnet script file, for example:

### Pre Commit

```csharp
#!/usr/bin/env dotnet-script
#r "./GitHooksCSharp.dll"

using GitHooksCsharp.Hooks.PreCommit;

PreCommitHook.ProjectPaths.Add("./Sample/Sample.csproj");
PreCommitHook.ProjectPaths.Add("./Sample2/Sample2.csproj");
PreCommitHook.TestProjectPaths.Add("./Sample.Test/Sample.Test.csproj");
PreCommitHook.TestProjectPaths.Add("./Sample2.Test/Sample2.Test.csproj");

PreCommitHook.Execute();
```
##### Prepare Commit Msg
```csharp
#!/usr/bin/env dotnet-script
#r "./GitHooksCSharp.dll"

using GitHooksCsharp.Hooks.PrepareCommitMsg;

PrepareCommitMsgHook.Execute();
```

##### Commit Msg
```csharp
#!/usr/bin/env dotnet-script
#r "./GitHooksCSharp.dll"

using GitHooksCsharp.Hooks.CommitMsg;

CommitMsgHook.Execute();
```

##### Commit Message Constraints
[cshooks-config.json](sample/GitHooksCSharp.Sample/cshooks-config.json) file to configure the validation used in `commit-msg`:
```json
{
  
  "subject": {
    "allowedTypes": [
      "feat",
      "fix",
      "chore",
      "test",
      "docs",
      "build",
      "ci",
      "style",
      "refactor",
      "perf",
      "revert"
    ],
    "allowedScopes": [
      "[\\w\\s!@\\#\\$%\\^\u0026\\*\\(\\)_=\\\u002B~\u0060\u0027\\[\\{}\\*\u00BF\\?\\|:;,\\.\u003E\u003C\\\\\\-\\]\\/]{2,20}"
    ],
    "isTypeRequired": true,
    "isScopeRequired": false,
    "descriptionRules": {
      "isRequired": true,
      "letterCase": "sentence",
      "minLength": 10,
      "maxLength": 70,
      "allowedSpecialChars": [],
      "mayEndWithPeriod": false,
      "mustEndWithPeriod": false
    }
  },
  
  "body": {
    "isRequired": false,
    "letterCase": "any",
    "minLength": 0,
    "maxLength": 0,
    "allowedSpecialChars": [],
    "mayEndWithPeriod": true,
    "mustEndWithPeriod": false
  },
  
  "footer": {
    "isRequired": false,
    "allowedKeywords": [
      "BREAKING CHANGE",
      "BREAKING-CHANGE",
      "[a-z]\u002B(?:-[a-z]\u002B)*"
    ],
    "descriptionRules": {
      "isRequired": true,
      "letterCase": "sentence",
      "minLength": 10,
      "maxLength": 0,
      "allowedSpecialChars": [],
      "mayEndWithPeriod": false,
      "mustEndWithPeriod": false
    }
  }
  
}
```

## License
This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

## Acknowledgments
- [Using C# code in your git hooks](https://kaylumah.medium.com/using-c-code-in-your-git-hooks-66e507c01a0f)
- [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/)
- [dotnet-script](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-install-script)