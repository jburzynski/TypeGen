using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWebApp.TestEntities;
using TypeGen.Core.TypeAnnotations;

namespace Core2WebApp.TestEntities
{
    [ExportTsInterface(OutputDir = "./very/nested/directory/")]
    public class NestedEntity
    {
        public GenericClass<string> GenericClassProperty { get; set; }

        [TsOptional]
        public string OptionalProperty { get; set; }
    }
}
