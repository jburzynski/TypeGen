namespace TypeGen.FileContentTest.Comments.Entities;

public class TsOuterClass
{
    /// <summary>
    /// This is the summary of a nested class.
    /// </summary>
    public class TsNestedClass
    {
        /// <summary>
        /// This property belongs to a nested class.
        /// </summary>
        public string StringProp { get; set; }

        /// <summary>
        /// This field belongs to a nested class.
        /// </summary>
        public string StringField;
    }
}