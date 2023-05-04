namespace TypeGen.IntegrationTest.Extensions;

public static class StringExtensions
{
    public static string FormatOutput(this string output)
        => output
            .Trim()
            .Replace("\n", "")
            .Replace("\r", "")
            .Replace("\r\n", "");
}