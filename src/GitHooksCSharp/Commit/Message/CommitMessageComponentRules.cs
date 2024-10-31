using System.Text.Json.Serialization;
using GitHooksCsharp.Text;

namespace GitHooksCsharp.Commit.Message;

/// <summary>
/// Represents the rules for a component of a commit message.
/// </summary>
public class CommitMessageComponentRules
{
    /// <summary>
    /// Gets or sets a value indicating whether the component is required.
    /// </summary>
    public bool IsRequired { get; set; }

    /// <summary>
    /// Gets or sets the letter case rule for the component.
    /// </summary>
    public LetterCase LetterCase { get; set; }

    /// <summary>
    /// Gets or sets the minimum length of the component.
    /// </summary>
    public int MinLength { get; set; }

    /// <summary>
    /// Gets or sets the maximum length of the component. A value of 0 means no limit.
    /// </summary>
    public int MaxLength { get; set; } // 0 means no limit.

    /// <summary>
    /// Gets or sets the allowed special characters for the component. If null or empty, any sign is allowed.
    /// </summary>
    public char[] AllowedSpecialChars { get; set; } = []; // If null or empty, any sign is allowed.

    /// <summary>
    /// Gets or sets a value indicating whether the component may end with a period.
    /// </summary>
    public bool MayEndWithPeriod { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the component must end with a period.
    /// </summary>
    public bool MustEndWithPeriod { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommitMessageComponentRules"/> class.
    /// </summary>
    /// <param name="isRequired">Indicates whether the component is required.</param>
    /// <param name="letterCase">The letter case rule for the component.</param>
    /// <param name="minLength">The minimum length of the component.</param>
    /// <param name="maxLength">The maximum length of the component.</param>
    /// <param name="mayEndWithPeriod">Indicates whether the component may end with a period.</param>
    /// <param name="mustEndWithPeriod">Indicates whether the component must end with a period.</param>
    public CommitMessageComponentRules(bool isRequired, LetterCase letterCase, int minLength, int maxLength, bool mayEndWithPeriod = true, bool mustEndWithPeriod = false)
    {
        IsRequired = isRequired;
        LetterCase = letterCase;
        MinLength = IsRequired ? Math.Max(1, minLength) : minLength;
        MaxLength = maxLength;
        MayEndWithPeriod = mayEndWithPeriod;
        MustEndWithPeriod = mustEndWithPeriod;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommitMessageComponentRules"/> class.
    /// </summary>
    /// <param name="isRequired">Indicates whether the component is required.</param>
    /// <param name="letterCase">The letter case rule for the component.</param>
    /// <param name="minLength">The minimum length of the component.</param>
    /// <param name="maxLength">The maximum length of the component.</param>
    /// <param name="allowedSpecialChars">The allowed special characters for the component.</param>
    public CommitMessageComponentRules(bool isRequired, LetterCase letterCase, int minLength, int maxLength, char[] allowedSpecialChars)
    {
        IsRequired = isRequired;
        LetterCase = letterCase;
        MinLength = IsRequired ? Math.Max(1, minLength) : minLength;
        MaxLength = maxLength;
        AllowedSpecialChars = allowedSpecialChars;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommitMessageComponentRules"/> class.
    /// </summary>
    /// <param name="isRequired">Indicates whether the component is required.</param>
    /// <param name="letterCase">The letter case rule for the component.</param>
    /// <param name="minLength">The minimum length of the component.</param>
    /// <param name="maxLength">The maximum length of the component.</param>
    /// <param name="allowedSpecialChars">The allowed special characters for the component.</param>
    /// <param name="mayEndWithPeriod">Indicates whether the component may end with a period.</param>
    /// <param name="mustEndWithPeriod">Indicates whether the component must end with a period.</param>
    [JsonConstructor]
    public CommitMessageComponentRules(bool isRequired, LetterCase letterCase, int minLength, int maxLength, char[] allowedSpecialChars, bool mayEndWithPeriod, bool mustEndWithPeriod)
    {
        IsRequired = isRequired;
        LetterCase = letterCase;
        MinLength = IsRequired ? Math.Max(1, minLength) : minLength;
        MaxLength = maxLength;
        AllowedSpecialChars = allowedSpecialChars;
        MayEndWithPeriod = mayEndWithPeriod;
        MustEndWithPeriod = mustEndWithPeriod;
    }
}