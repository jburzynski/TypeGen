using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using TypeGen.Core;

namespace TypeGen.Cli
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Assembly assembly;
            string projectFolder = string.Empty;

            if (args.Contains("--load-tg-test"))
            {
                assembly = Assembly.LoadFrom("TypeGen.Test.dll");
            }
            else
            {
                projectFolder = args[0];

                string csProjFileName = Directory.GetFiles(projectFolder)
                    .Select(GetFileNameFromPath)
                    .FirstOrDefault(n => n.EndsWith(".csproj"));

                string debugFolder = projectFolder + "\\bin\\Debug\\";

                string assemblyFileName = ChangeFileExtension(csProjFileName, "dll");
                if (!File.Exists(debugFolder + assemblyFileName))
                {
                    assemblyFileName = ChangeFileExtension(csProjFileName, "exe");
                }

                assembly = csProjFileName != null ? Assembly.LoadFrom(debugFolder + assemblyFileName) : null;
            }

            var generator = new Generator(projectFolder);
            generator.Generate(assembly);
        }

        private static string GetFileNameFromPath(string path)
        {
            return path.Split('\\').Last();
        }

        private static string ChangeFileExtension(string fileName, string toExt)
        {
            string fileNameNoExt = fileName.Split('.').First();
            return fileNameNoExt + "." + toExt;
        }
    }
}
