#nullable enable
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.GenericInheritance.Entities;

[ExportTsInterface]
public class GenericServiceResponseDto<T>
{
    [TsOptional]
    public T? Data { get; set; }
}