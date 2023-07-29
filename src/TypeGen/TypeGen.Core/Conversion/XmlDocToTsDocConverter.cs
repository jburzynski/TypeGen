using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using TypeGen.Core.Extensions;

namespace TypeGen.Core.Conversion;

internal class XmlDocToTsDocConverter
{
    public static string Convert(string xmlDoc)
    {
        if (xmlDoc == null) throw new ArgumentNullException(nameof(xmlDoc));
        
        var tsDoc = ConvertSummary(xmlDoc);
        tsDoc = ConvertExample(tsDoc);
        tsDoc = ConvertRemarks(tsDoc);
        tsDoc = ConvertSee(tsDoc);
        tsDoc = ConvertTypeParam(tsDoc);
        tsDoc = ConvertInheritDoc(tsDoc);
        tsDoc = tsDoc.SetIndentation(0);
        return WrapInTsComments(tsDoc);
    }

    private static string WrapInTsComments(string str)
    {
        str = str.Trim().NormalizeNewLines();
        var strLines = str.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        var body = string.Join(Environment.NewLine, strLines.Select(x => $" * {x}"));
        return $"/**{Environment.NewLine}{body}{Environment.NewLine} */";
    }

    private static string ConvertSummary(string str)
        => ConvertTags(str, $"""<summary>(.*?)<\/summary>""", "$1");
    
    private static string ConvertExample(string str)
    {
        var result = ConvertTags(str, $"""<example>(.*?)<\/example>""", $"@example{Environment.NewLine}$1");
        result = result.Replace($"@example{Environment.NewLine}{Environment.NewLine}", $"@example{Environment.NewLine}");
        return EnsureLineMargin(result, "@example", 2, 2);
    }

    private static string ConvertRemarks(string str)
    {
        var result = ConvertTags(str, $"""<remarks>(.*?)<\/remarks>""", $"@remarks{Environment.NewLine}$1");
        result = result.Replace($"@remarks{Environment.NewLine}{Environment.NewLine}", $"@remarks{Environment.NewLine}");
        return EnsureLineMargin(result, "@remarks", 2, 2);
    }

    private static string ConvertSee(string str)
        => ConvertTags(str, $"""<see cref=".:(.*?)"[ ]*?\/>""", "@see {@link $1}");
    
    private static string ConvertTypeParam(string str)
    {
        var result = ConvertTags(str, $"""<typeparam name="(.*?)">(.*?)<\/typeparam>""", "@typeParam $1 - $2");
        return EnsureLineMargin(result, "@typeParam", 2, 1);
    }

    private static string ConvertInheritDoc(string str)
    {
        var result = ConvertTags(str, $"""<inheritdoc[ ]*?\/>""", "{@inheritDoc}");
        return ConvertTags(result, $"""<inheritdoc cref="(.*?)"[ ]*?\/>""", "{@inheritDoc $1}");
    }

    private static string ConvertTags(string str, string regexPattern, string replacement)
        => Regex.Replace(str, regexPattern, replacement, RegexOptions.Singleline);

    private static string EnsureLineMargin(string input, string tag, int firstElementMargin, int subsequentElementsMargin)
    {
        var regex = new Regex($"\\s*?{tag}");
        
        var firstElementNewLines = string.Join("", Enumerable.Repeat(Environment.NewLine, firstElementMargin));
        var subsequentElementsNewLines = string.Join("", Enumerable.Repeat(Environment.NewLine, subsequentElementsMargin));
        
        var result = regex.Replace(input, $"{subsequentElementsNewLines}{tag}");
        return regex.Replace(result, $"{firstElementNewLines}{tag}", 1);
    }
}