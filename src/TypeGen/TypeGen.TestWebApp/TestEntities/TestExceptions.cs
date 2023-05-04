#nullable enable
using System;
using System.Collections.Generic;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.TestEntities
{
    public class TestExceptions
    {
        public List<Guid?>? InvalidCollection { get; set; }
    }
}
