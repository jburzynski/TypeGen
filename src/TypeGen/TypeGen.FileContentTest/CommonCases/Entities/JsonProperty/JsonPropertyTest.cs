using Newtonsoft.Json;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.FileContentTest.CommonCases.Entities.JsonProperty
{
    [ExportTsClass(OutputDir = "json-property-test")]
    public class JsonPropertyTest
    {
        [JsonProperty("MY_PROPERTY")]
        public string MyProperty { get; set; }
    }
}