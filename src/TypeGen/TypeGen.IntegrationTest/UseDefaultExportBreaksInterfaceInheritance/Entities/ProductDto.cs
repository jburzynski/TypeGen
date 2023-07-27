namespace TypeGen.IntegrationTest.UseDefaultExportBreaksInterfaceInheritance.Entities;

public class ProductDto : IId, IIdentifier
{
    public int Id { get; set; }
    public string Identifier { get; set; }
}