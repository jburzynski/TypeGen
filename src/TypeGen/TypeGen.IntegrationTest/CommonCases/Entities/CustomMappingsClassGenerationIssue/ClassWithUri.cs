using System;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.IntegrationTest.CommonCases.Entities.CustomMappingsClassGenerationIssue;

[ExportTsClass]
public class ClassWithUri
{
    public Uri MyUri { get; set; }
}