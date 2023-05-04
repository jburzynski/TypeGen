using System;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.CustomMappingsClassGenerationIssue;

[ExportTsClass]
public class ClassWithUri
{
    public Uri MyUri { get; set; }
}