using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using TypeGen.Core.Storage;
using TypeGen.Core.Utils;
using TypeGen.Core.Validation;

namespace TypeGen.Core.Business
{
    /// <summary>
    /// Parses TypeScript file contents
    /// </summary>
    internal class TsContentParser : ITsContentParser
    {
        private readonly IFileSystem _fileSystem;

        public TsContentParser(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        /// <summary>
        /// Gets text within specified tag(s).
        /// If tag(s) occurs multiple times, concatenated text from all occurrences is returned.
        /// Returns an empty string if a file does not exist.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="indentSize"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public string GetTagContent(string filePath, int indentSize, params string[] tags)
        {
            Requires.NotNull(filePath, nameof(filePath));
            Requires.NotNullOrEmpty(tags, nameof(tags));
            
            if (!_fileSystem.FileExists(filePath)) return "";
            string content = _fileSystem.ReadFile(filePath);

            string tagRegex = $"({string.Join("|", tags)})";
            MatchCollection matches = Regex.Matches(content, $@"\/\/<{tagRegex}>((.|\n|\r|\r\n)+?)\/\/<\/{tagRegex}>", RegexOptions.IgnoreCase);

            string indent = StringUtils.GetTabText(indentSize);

            string result = matches
                .Cast<Match>()
                .Aggregate("", (current, match) => current + $"{indent}{match.Groups[2].Value.Trim()}\r\n");

            if (!string.IsNullOrEmpty(result))
            {
                result = result.Remove(0, indentSize);
            }

            return result;
        }
    }
}
