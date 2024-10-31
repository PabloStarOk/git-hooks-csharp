using System.Text.Json.Serialization;
using GitHooksCsharp.Text;
using GitHooksCsharp.Utils;

namespace GitHooksCsharp.Commit.Message;

/// <summary>
/// Represents the rules for the footer of a commit message.
/// </summary>
public class FooterRules
{
    /// <summary>
    /// Gets or sets a value indicating whether the footer is required.
    /// </summary>
    public bool IsRequired { get; set; }

    /// <summary>
    /// Gets the allowed keywords for the footer.
    /// </summary>
    public List<string> AllowedKeywords { get; } = ["BREAKING CHANGE", "BREAKING-CHANGE", RegexUtils.HyphenatedWords];

    /// <summary>
    /// Gets the rules for the description of the footer.
    /// </summary>
    public CommitMessageComponentRules DescriptionRules { get; } = new(true, LetterCase.Sentence, 10, 0, false);

    /// <summary>
    /// Initializes a new instance of the <see cref="FooterRules"/> class.
    /// </summary>
    public FooterRules() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="FooterRules"/> class.
    /// </summary>
    /// <param name="isRequired">Indicates whether the footer is required.</param>
    /// <param name="allowedKeywords">The allowed keywords for the footer.</param>
    /// <param name="descriptionRules">The rules for the description of the footer.</param>
    [JsonConstructor]
    public FooterRules(bool isRequired, List<string> allowedKeywords,
        CommitMessageComponentRules descriptionRules)
    {
        IsRequired = isRequired;
        AllowedKeywords = allowedKeywords;
        DescriptionRules = descriptionRules;
    }
}