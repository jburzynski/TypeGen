using System.Collections.Generic;
using System.Linq;
using System.Xml;
using TypeGen.Core.Storage;
using TypeGen.Core.Validation;

namespace TypeGen.Cli.ProjectFileManagement
{
    /// <summary>
    /// For ASP.NET (.NET Framework) versions (addFilesToProject parameter in TypeGen CLI)
    /// </summary>
    internal class ProjectFileManager : IProjectFileManager
    {
        private const string TypeScriptCompileXPath = "/*[local-name()='Project']/*[local-name()='ItemGroup']/*[local-name()='TypeScriptCompile']";

        private readonly IFileSystem _fileSystem;

        public ProjectFileManager(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public bool ContainsTsFile(XmlDocument projectDocument, string filePath)
        {
            Requires.NotNull(projectDocument, nameof(projectDocument));

            XmlNodeList itemGroups = projectDocument.DocumentElement?.SelectNodes($"{TypeScriptCompileXPath}[@Include='{filePath}']");

            return itemGroups != null && itemGroups.Count > 0;
        }

        public XmlDocument ReadFromProjectFolder(string projectFolder)
        {
            string projectFilePath = GetProjectPath(projectFolder);
            if (string.IsNullOrEmpty(projectFilePath)) return null;

            var document = new XmlDocument();
            document.Load(projectFilePath);

            return document;
        }

        public void SaveProjectFile(string projectFolder, XmlDocument projectFile)
        {
            string projectFilePath = GetProjectPath(projectFolder);
            projectFile.Save(projectFilePath);
        }

        private string GetProjectPath(string projectFolder)
        {
            return _fileSystem.GetDirectoryFiles(projectFolder)
                .FirstOrDefault(x => x.EndsWith(".csproj"));
        }

        public void AddTsFiles(XmlDocument projectDocument, IEnumerable<string> filePaths)
        {
            foreach (var filePath in filePaths)
                AddTsFile(projectDocument, filePath);
        }

        public void AddTsFile(XmlDocument projectDocument, string filePath)
        {
            Requires.NotNull(projectDocument, nameof(projectDocument));

            XmlElement documentElement = projectDocument.DocumentElement;
            if (documentElement == null) throw new CliException("Project file has no XML document element");

            if (ContainsTsFile(projectDocument, filePath)) return;

            XmlNode itemGroupNode = documentElement
                .SelectSingleNode(TypeScriptCompileXPath)
                ?.ParentNode
                ?? AddItemGroup(projectDocument);

            itemGroupNode.AppendChild(
                CreateItemGroupChild(projectDocument, "TypeScriptCompile", filePath)
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
