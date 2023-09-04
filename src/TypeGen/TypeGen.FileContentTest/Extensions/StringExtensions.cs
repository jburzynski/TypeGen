namespace TypeGen.FileContentTest.Extensions;

public static class StringExtensions
{
    public static string NormalizeFileContent(this string content)
        => content
            .Trim('\uFEFF', '\u200B')
            .Trim()
            .Replace("\n", "")
            .Replace("\r", "")
            .Replace("\r\n", "");
}