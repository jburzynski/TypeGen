using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreWebApp.TestEntities;
using TypeGen.Core.TypeAnnotations;

namespace CoreWebApp.ErrorCase2
{
    [ExportTsClass(OutputDir = "error-case2")]
    public class MyClass : BaseClass<Guid>
    {
        public MyClass MyMember { get; set; }
        [TsType(TsType.String)]
        public Guid? MyMemberId { get; set; }

        public List<MyJoinClass> MyJoins { get; set; }
    }

    public class MyJoinClass
    {
        public MyClass Class { get; set; }
        [TsType(TsType.String)]
        public Guid ClassId { get; set; }

        public TestClass<string, BaseClass2<string>> OtherClass { get; set; }
        [TsType(TsType.String)]
        public Guid OtherClassId { get; set; }
    }
}
