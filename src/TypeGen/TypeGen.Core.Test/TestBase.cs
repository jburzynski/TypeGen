using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute;

namespace TypeGen.Core.Test
{
    public class TestBase
    {
        protected T GetInstance<T>() where T: class => Substitute.For<T>();
    }
}
