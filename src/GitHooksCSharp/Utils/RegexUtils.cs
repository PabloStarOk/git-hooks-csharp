using System.Text.RegularExpressions;
using GitHooksCsharp.Text;

namespace GitHooksCsharp.Utils;

/// <summary>
/// Provides regex utils for commit messages.
/// </summary>
public static class RegexUtils
{
    public const string AnyText = @"[a-zA-Z]+"; // Any word. E.g. "someWord"
    public const string AnyTextNum = @"[a-zA-Z0-9]+"; // Any word with numbers. E.g. "someWord123"
    public const string AnyTextSpecial = @"[a-zA-Z!@#$%^&*()_+]+"; // Any word with special characters. E.g. "someWord!"
    public const string AnyTextNumSpecial = @"[a-zA-Z0-9!@#$%^&*()_+]+"; // Any word with numbers and special characters. E.g. "someWord123!"
    
    public const string SentenceCaseText = @"^[A-Z][a-z]*(?:\s+[a-z]+)*$";
    public const string TitleCaseText = @"^([A-Z][a-z]*)(\s+[A-Z][a-z]*)*$";
    public const string LowerCaseText = @"^[a-z\s]+$";
    public const string UpperCaseText = @"^[A-Z\s]+$";
    
    public const string LatinAccents = "[À-ÿ]";
    public readonly static string SpecialChars = Regex.Escape(@"!@#$%^&*()_=+~`'[{}*¿?|:;,.><\-]/");
    public const string ControlChars = @"\x00-\x1F";
    
    public const string HyphenatedWords = "[a-z]+(?:-[a-z]+)*";

    public static string GetLetterCaseRegex(LetterCase letterCase, int minLength, int maxLength, params char[] allowedSpecialChars)
    {
        string specialChars = FormatSpecialChars(allowedSpecialChars.Length > 0 
            ? allowedSpecialChars 
            : SpecialChars.ToCharArray());
        string range = GetRange(minLength, maxLength);
        
        return letterCase switch
        {
            LetterCase.Lower => @$"[a-z\s{specialChars}]{range}",
            LetterCase.Upper => @$"[A-Z\s{specialChars}]{range}",
            LetterCase.Sentence => @$"[A-Z][\w\s{specialChars}]{range}",
            LetterCase.Title => @$"([A-Z][\w{specialChars}]*)(?:\s[A-Z][\w{specialChars}]*)*",
            _ => @$"[\w\s{specialChars}]{range}"
        };
    }

    private static string FormatSpecialChars(char[] chars)
    {
        if (chars.Length == 0)
            return "";
        
        string specialChars = Regex.Escape(new string(chars));
        
        // Remove extra backslashes added by Validation.Escape()
        if (specialChars.Contains(']') || specialChars.Contains('/'))
            specialChars = specialChars.Replace("]", @"\]")
                                        .Replace("/", @"\/")
                                        .Replace("-", @"\-")
                                        .Replace(@"\\\", @"\");
        
        return specialChars;
    }
    
    private static string GetRange(int min, int max)
    {
        if (min != 0 && max <= 0)
            return "+";
        else if (min == 0 && max == 0)
            return "*";
        else
            return $"{{{min},{max}}}";
    }
}