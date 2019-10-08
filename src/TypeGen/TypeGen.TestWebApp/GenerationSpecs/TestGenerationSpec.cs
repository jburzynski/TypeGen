using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TypeGen.Core.Extensions;
using TypeGen.Core.SpecGeneration;
using TypeGen.Core.TypeAnnotations;

namespace TypeGen.TestWebApp.GenerationSpecs
{
    public class TestGenerationSpec : GenerationSpec
    {
        public override void OnBeforeBarrelGeneration(OnBeforeBarrelGenerationArgs args)
        {
            IEnumerable<string> directories = GetAllDirectoriesRecursive(args.GeneratorOptions.BaseOutputDirectory)
                .Select(x => GetPathDiff(args.GeneratorOptions.BaseOutputDirectory, x));

            foreach (string directory in directories)
            {
                AddBarrel(directory);
            }

            AddBarrel(".");

//            AddBarrel("test-classes");
//            AddBarrel("default-export", BarrelScope.Directories);
//            AddBarrel("very");
//            AddBarrel("very/nested/directory");
//            AddBarrel("no", BarrelScope.Files);
//            AddBarrel("test-interfaces", BarrelScope.Directories);
        }

        private string GetPathDiff(string pathFrom, string pathTo)
        {
            var pathFromUri = new Uri("file:///" + pathFrom?.Replace('\\', '/'));
            var pathToUri = new Uri("file:///" + pathTo?.Replace('\\', '/'));

            return pathFromUri.MakeRelativeUri(pathToUri).ToString();
        }

        private IEnumerable<string> GetAllDirectoriesRecursive(string directory)
        {
            var result = new List<string>();
            string[] subdirectories = Directory.GetDirectories(directory);

            if (!subdirectories.Any()) return result;
            
            result.AddRange(subdirectories);

            foreach (string subdirectory in subdirectories)
            {
                result.AddRange(GetAllDirectoriesRecursive(subdirectory));
            }

            return result;
        }
    }
}