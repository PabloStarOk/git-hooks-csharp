namespace GitHooksCsharp.Utils;

public static class CmdUtils
{
    public static string GetCommandLineArg(IList<string> Args, int position)
    {
        return Args.Count >= position + 1 ? Args[position] : string.Empty;
    }
}