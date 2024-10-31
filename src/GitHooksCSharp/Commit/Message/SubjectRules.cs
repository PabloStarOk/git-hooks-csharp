using System.Text.Json.Serialization;
using GitHooksCsharp.Text;
using GitHooksCsharp.Utils;

namespace GitHooksCsharp.Commit.Message;

/// <summary>
/// Represents the rules for the subject of a commit message.
/// </summary>
public class SubjectRules
{
    /// <summary>
    /// Gets the allowed types for the subject.
    /// </summary>
    public List<string> AllowedTypes { get; } = ["feat", "fix", "chore", "test", "docs", "build", "ci", "style", "refactor", "perf", "revert"];

    /// <summary>
    /// Gets the allowed scopes for the subject.
    /// </summary>
    public List<string> AllowedScopes { get; } = [RegexUtils.GetLetterCaseRegex(LetterCase.Any, 2, 20)];

    /// <summary>
    /// Gets or sets a value indicating whether the type is required.
    /// </summary>
    public bool IsTypeRequired { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the scope is required.
    /// </summary>
    public bool IsScopeRequired { get; set; }

    /// <summary>
    /// Gets the rules for the description of the subject.
    /// </summary>
    public CommitMessageComponentRules DescriptionRules { get; } = new(true, LetterCase.Sentence, 10, 70, false);

    /// <summary>
    /// Initializes a new instance of the <see cref="SubjectRules"/> class.
    /// </summary>
    public SubjectRules() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="SubjectRules"/> class.
    /// </summary>
    /// <param name="allowedTypes">The allowed types for the subject.</param>
    /// <param name="allowedScopes">The allowed scopes for the subject.</param>
    /// <param name="isTypeRequired">Indicates whether the type is required.</param>
    /// <param name="isScopeRequired">Indicates whether the scope is required.</param>
    /// <param name="descriptionRules">The rules for the description of the subject.</param>
    [JsonConstructor]
    public SubjectRules(List<string> allowedTypes, List<string> allowedScopes,
        bool isTypeRequired, bool isScopeRequired, CommitMessageComponentRules descriptionRules)
    {
        AllowedTypes = allowedTypes;
        AllowedScopes = allowedScopes;
        IsTypeRequired = isTypeRequired;
        IsScopeRequired = isScopeRequired;
        DescriptionRules = descriptionRules;
    }
}