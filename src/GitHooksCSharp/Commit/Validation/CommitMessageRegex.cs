using GitHooksCsharp.Commit.Message;
using GitHooksCsharp.Logging;
using GitHooksCsharp.Text;
using GitHooksCsharp.Utils;

namespace GitHooksCsharp.Commit.Validation;

/// <summary>
/// Provides regular expressions for validating commit messages.
/// </summary>
public static class CommitMessageRegex
{
    #region Validation
    /// <summary>
    /// Gets the regular expression for validating the subject of a commit message.
    /// </summary>
    public static string SubjectRegex { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the regular expression for validating the body of a commit message.
    /// </summary>
    public static string BodyRegex { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the regular expression for validating the footer of a commit message.
    /// </summary>
    public static string FooterRegex { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the regular expression for validating the keyword in the footer of a commit message.
    /// </summary>
    public static string FooterKeywordRegex { get; private set; } = string.Empty;
    #endregion

    #region Msg Template
    /// <summary>
    /// Template for the subject of a commit message.
    /// </summary>
    public const string SubjectTemplate = $"<type>(optional scope): <description> <optional ISS-XXX>";

    /// <summary>
    /// Template for the body of a commit message.
    /// </summary>
    public const string BodyTemplate = $"<optional body>";

    /// <summary>
    /// Template for the footer of a commit message.
    /// </summary>
    public const string FooterTemplate = "<key-word>: <footer description>";
    #endregion

    /// <summary>
    /// Static constructor to initialize the regular expressions.
    /// </summary>
    static CommitMessageRegex()
    {
        FillSubjectRegex();
        FillBodyRegex();
        FillFooterRegex();
        
        Logger.BlankLine();
        Logger.LogWarning("☢️ Generated regex to validate ☢️");
        Logger.LogInfo($"Subject regex: {SubjectRegex}");
        Logger.LogInfo($"Body regex: {BodyRegex}");
        Logger.LogInfo($"Footer regex: {FooterRegex}");
    }

    /// <summary>
    /// Fills the regular expression for validating the subject of a commit message.
    /// </summary>
    private static void FillSubjectRegex()
    {
        // Type Validation
        string typeRequired = Configuration.CommitMessageRules.Subject.IsTypeRequired ? "" : "?";
        string typeRegex = $"^({string.Join('|', Configuration.CommitMessageRules.Subject.AllowedTypes)}){typeRequired}";

        // Scope Validation
        string scopeRequired = Configuration.CommitMessageRules.Subject.IsScopeRequired ? "" : "?";
        string scopeRegex = @$"(\(\b({string.Join('|', Configuration.CommitMessageRules.Subject.AllowedScopes)})\b\)){scopeRequired}";

        const string breakChangeRegex = "!?";
        string divisorRequired = Configuration.CommitMessageRules.Subject.IsTypeRequired
            || Configuration.CommitMessageRules.Subject.IsScopeRequired ? "" : "?";
        string divisorRegex = $@"(:\s){divisorRequired}";
        string descriptionRegex = BuildDescriptionRegex(Configuration.CommitMessageRules.Subject.DescriptionRules);

        SubjectRegex = typeRegex + scopeRegex + breakChangeRegex + divisorRegex + descriptionRegex;
    }

    /// <summary>
    /// Fills the regular expression for validating the body of a commit message.
    /// </summary>
    private static void FillBodyRegex()
    {
        BodyRegex = "^" + BuildDescriptionRegex(Configuration.CommitMessageRules.Body);
    }

    /// <summary>
    /// Fills the regular expression for validating the footer of a commit message.
    /// </summary>
    private static void FillFooterRegex()
    {
        string[] allowedKeywords = Configuration.CommitMessageRules.Footer.AllowedKeywords.ToArray();
        string keywordRegex = $"^({string.Join('|', allowedKeywords)})";
        const string divisorRegex = @":\s";
        string footerDescriptionRegex = BuildDescriptionRegex(Configuration.CommitMessageRules.Footer.DescriptionRules);

        FooterKeywordRegex = keywordRegex + ":"; // Add colon to the end of the keyword because it's used to identify footers.
        FooterRegex = keywordRegex + divisorRegex + footerDescriptionRegex; // Used to check the whole format of a footer.
    }

    /// <summary>
    /// Builds a regular expression for validating a description component of a commit message.
    /// </summary>
    /// <param name="descriptionRules">The rules for the description component.</param>
    /// <returns>The regular expression for the description component.</returns>
    private static string BuildDescriptionRegex(CommitMessageComponentRules descriptionRules)
    {
        string isRequired = descriptionRules.IsRequired ? "" : "?";
        bool isLowerCase = descriptionRules.LetterCase == LetterCase.Lower;
        bool isUpperCase = descriptionRules.LetterCase == LetterCase.Upper;
        string textRules = RegexUtils.GetLetterCaseRegex(descriptionRules.LetterCase,
            descriptionRules.MinLength, descriptionRules.MaxLength, descriptionRules.AllowedSpecialChars);
        string mayEndWithPeriod = descriptionRules.MayEndWithPeriod ? "" : ".";

        // Handle not allowed letter case when using lower or upper case.
        string notAllowedLetterCase = string.Empty;
        if (isLowerCase)
            notAllowedLetterCase = "A-Z";
        else if (isUpperCase)
            notAllowedLetterCase = "a-z";

        // Build the whole regex
        string finalRegex = descriptionRules.MustEndWithPeriod ?
            $@"({textRules}[^{notAllowedLetterCase}\s]\.$){isRequired}" :
            $@"({textRules}[^{mayEndWithPeriod}{notAllowedLetterCase}\s]){isRequired}$";

        return finalRegex;
    }
}