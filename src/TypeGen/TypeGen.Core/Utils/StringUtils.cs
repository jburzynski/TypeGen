namespace TypeGen.Core.Utils
{
    /// <summary>
    /// String-related utility class
    /// </summary>
    internal static class StringUtils
    {
        /// <summary>
        /// Gets a string value to use as a tab text
        /// </summary>
        /// <param name="tabLength">The number of spaces per tab.</param>
        /// <returns></returns>
        public static string GetTabText(int tabLength)
        {
            var tabText = "";
            for (var i = 0; i < tabLength; i++)
            {
                tabText += " ";
            }
            return tabText;
        }
    }
}
