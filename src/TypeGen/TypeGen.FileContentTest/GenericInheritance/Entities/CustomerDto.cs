#nullable enable
using System;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.GenericInheritance.Entities;

[ExportTsInterface]
public class CustomerDto
{
    public DateTime CreatedAt { get; set; }
}