using System.Xml;

namespace TypeGen.Cli.ProjectFileManagement
{
    internal interface IProjectFileManager
    {
        bool ContainsTsFile(XmlDocument projectFile, string filePath);
        XmlDocument ReadFromProjectFolder(string projectFolder);
        void SaveProjectFile(string projectFolder, XmlDocument projectFile);
        void AddTsFile(XmlDocument projectFile, string filePath);
    }
}