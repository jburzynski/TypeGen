using System;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities.CustomMappingsClassGenerationIssue;

[ExportTsClass]
public class ClassWithUri
{
    public Uri MyUri { get; set; }
}