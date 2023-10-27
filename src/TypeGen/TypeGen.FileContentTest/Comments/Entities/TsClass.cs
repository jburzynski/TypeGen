namespace TypeGen.FileContentTest.Comments.Entities;

/// <summary>
/// This is the summary of TsClass.
/// It has many lines.
/// This one is the last one. See <see cref="TsClass"/> for more details.
/// </summary>
/// <example>
/// This is how you can code: <see cref="http://example.com"/>.
/// </example>
/// <example>
/// Another example.
/// </example>
/// <remarks>I have no remarks.</remarks>
public class TsClass
{
    /// <summary>
    /// Please use this property.
    /// </summary>
    public string StringProp { get; set; }

    /// <summary>
    /// This property is good.
    /// </summary>
    public int IntProp { get; set; }

    /// <summary>
    /// Let's generate some fields.
    /// </summary>
    public string StringField;
    
    public TsEnum NoComment { get; set; }
}