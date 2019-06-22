namespace TypeGen.Core.Generator.Services
{
    internal interface ITsContentParser
    {
        /// <summary>
        /// Gets text within specified tag(s).
        /// If tag(s) occurs multiple times, concatenated text from all occurrences is returned.
        /// Returns an empty string if a file does not exist.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="indentSize"></param>
        /// <param name="tags"></param>
        /// <returns></returns>
        string GetTagContent(string filePath, int indentSize, params string[] tags);
    }
}