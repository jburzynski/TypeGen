using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TypeGen.Core;

namespace TypeGen.Cli
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Assembly assembly = Assembly.LoadFrom("TypeGen.Test.dll");
            var generator = new Generator();
            generator.GenerateFromAssembly(assembly);
        }
    }
}
