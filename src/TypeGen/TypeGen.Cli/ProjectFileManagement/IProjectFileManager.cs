using System.Collections.Generic;
using System.Xml;

namespace TypeGen.Cli.ProjectFileManagement
{
    internal interface IProjectFileManager
    {
        bool ContainsTsFile(XmlDocument projectDocument, string filePath);
        XmlDocument ReadFromProjectFolder(string projectFolder);
        void SaveProjectFile(string projectFolder, XmlDocument projectFile);
        void AddTsFile(XmlDocument projectDocument, string filePath);
        void AddTsFiles(XmlDocument projectDocument, IEnumerable<string> filePaths);
    }
}