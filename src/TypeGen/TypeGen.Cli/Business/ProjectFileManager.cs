using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using TypeGen.Cli.Extensions;
using TypeGen.Core.Utils;

namespace TypeGen.Cli.Business
{
    internal class ProjectFileManager
    {
        private const string TypeScriptCompileXPath = "/*[local-name()='Project']/*[local-name()='ItemGroup']/*[local-name()='TypeScriptCompile']";

        private readonly FileSystem _fileSystem;

        public ProjectFileManager(FileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        private string GetProjectFileName(string projectFolder)
        {
            return _fileSystem.GetDirectoryFiles(projectFolder)
                .Select(FileSystemUtils.GetFileNameFromPath)
                .FirstOrDefault(n => n.EndsWith(".csproj"));
        }

        public bool ContainsTsFile(XmlDocument projectFile, string filePath)
        {
            if (projectFile == null) throw new ArgumentNullException(nameof(projectFile));

            XmlNodeList itemGroups = projectFile.DocumentElement?.SelectNodes($"{TypeScriptCompileXPath}[@Include='{filePath}']");

            return itemGroups != null && itemGroups.Count > 0;
        }

        public XmlDocument ReadFromProjectFolder(string projectFolder)
        {
            string projectFileName = GetProjectFileName(projectFolder);
            if (string.IsNullOrEmpty(projectFileName)) return null;

            string xmlPath = projectFolder.ConcatPath(projectFileName);

            var document = new XmlDocument();
            document.Load(xmlPath);

            return document;
        }

        public void SaveProjectFile(string projectFolder, XmlDocument projectFile)
        {
            string projectFileName = GetProjectFileName(projectFolder);
            string filePath = projectFolder.ConcatPath(projectFileName);

            projectFile.Save(filePath);
        }

        public void AddTsFile(XmlDocument projectFile, string filePath)
        {
            if (projectFile == null) throw new ArgumentNullException(nameof(projectFile));

            XmlElement documentElement = projectFile.DocumentElement;
            if (documentElement == null) throw new CliException("Project file has no XML document element");

            if (ContainsTsFile(projectFile, filePath)) return;

            XmlNode itemGroupNode = documentElement
                .SelectSingleNode(TypeScriptCompileXPath)
                ?.ParentNode
                ?? AddItemGroup(projectFile);

            itemGroupNode.AppendChild(
                CreateItemGroupChild(projectFile, "TypeScriptCompile", filePath)
                );
        }

        private XmlNode AddItemGroup(XmlDocument document)
        {
            XmlNodeList itemGroups = document.DocumentElement.SelectNodes("/*[local-name()='Project']/*[local-name()='ItemGroup']");
            if (itemGroups == null || itemGroups.Count == 0) throw new CliException("Project file has no ItemGroups");

            XmlNode lastItemGroup = itemGroups.Item(itemGroups.Count - 1);
            XmlElement result = document.CreateElement("ItemGroup", document.DocumentElement.NamespaceURI);

            lastItemGroup.ParentNode.InsertAfter(result, lastItemGroup);
            return result;
        }

        private XmlNode CreateItemGroupChild(XmlDocument document, string name, string include)
        {
            XmlElement result = document.CreateElement(name, document.DocumentElement.NamespaceURI);

            XmlAttribute includeAttribute = document.CreateAttribute("Include");
            includeAttribute.InnerText = include;

            result.Attributes.Append(includeAttribute);

            return result;
        }
    }
}
