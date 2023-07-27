namespace TypeGen.TestWebApp.UseDefaultExportBreaksInterfaceInheritance128;

public class ProductDto : IId, IIdentifier
{
    public int Id { get; set; }
    public string Identifier { get; set; }
}