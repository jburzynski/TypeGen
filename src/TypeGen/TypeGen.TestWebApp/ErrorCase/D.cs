using System;
using System.Collections.Generic;
using TypeGen.Core.TypeAnnotations;
using TypeGen.TestWebApp.TestEntities;

namespace TypeGen.TestWebApp.ErrorCase
{
    public class D : GenericClass<int>
    {
        public TestEnum GetCurrentStatus(string a)
        {
            return TestEnum.A;
        }

        [TsType(TsType.String)]
        public Guid? AId { get; set; }

        public TestInterface A { get; set; }

        [TsType(TsType.String)]
        public Guid? BId { get; set; }

        public int? CId { get; set; }
        public virtual TestInterface C { get; set; } // DTypeClass is big with mostly string and int members

        public int? BarId { get; set; }
        public Bar Bar { get; set; }

        public string E
        {
            get { return "asdf"; }
        }

        // A here is same class as in Bar definition
        public D Init(TestInterface a, FooType footype)
        {
            return null;
        }

        public void G()
        {
            // logic removed
        }

        public string H()
        {
            return "asdf";
        }
        public string I()
        {
            return "adsf";
        }

        public DateTime? J { get; set; }
        public DateTime? K { get; set; }

        public IList<FClass> Fs { get; set; } = new List<FClass>();

        public DateTime? L
        {
            get;
            set;
        }

        public DateTime? M
        {
            get;
            set;
        }

        public TestEnum N
        {
            get;
            set;
        }

        [TsType(TsType.String)]
        public Guid? OId { get; set; }
    }
}
