using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeGen.Core.Converters;

namespace TypeGen.IntegrationTest
{
    internal class AddFolderConverter : ITypeNameConverter
    {
        public string Convert(string name, Type type)
        {
            return "foobar" + Path.DirectorySeparatorChar + name;
        }
    }
}
