#nullable enable
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.GenericInheritance.Entities;

[ExportTsInterface]
public class GetCustomersResponseDto : GenericServiceResponseDto<CustomerDto?>
{
    
}