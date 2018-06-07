using System;
using System.IO;
using System.Linq;
using System.Xml;
using NugetPublishExtTool.Models;
using NugetPublishExtTool.Definitions;
using NugetPublishExtTool.Utils;

namespace NugetPublishExtTool.Services
{
    public class NugetPublishService
    {
        public static void DoPublish(SelectedProjectInfo selectedProject, NugetInfo projectNugetInfo)
        {
            //            if (!File.Exists(@"..\.nuget\NuGet.exe"))
            //            {
            //                //TODO:提示用户没有..\.nuget\NuGet.exe这个文件
            //                return;
            //            }

            GetDebugOrReleaseOutPutPath(selectedProject);
        }

        private static void GetDebugOrReleaseOutPutPath(SelectedProjectInfo selectedProject)
        {
            try
            {
                var csprojXmlDoc = new XmlDocument();
                csprojXmlDoc.Load(selectedProject.ProjectFileFullPath);
                var csprojRoot = csprojXmlDoc.DocumentElement;
                string nameSpace = csprojRoot?.NamespaceURI;
                var nsmgr = new XmlNamespaceManager(csprojXmlDoc.NameTable);
                nsmgr.AddNamespace(CsprjFileDefinition.CsprjXmlNameSpaceAliasName, nameSpace);

                var propertyGroupList = XmlUtil
                    .GetXmlNodesValue(csprojRoot, CsprjFileDefinition.PropertyGroupXpath, nsmgr, XmlValueType.OuterXml)
                    .ToList();
                foreach (var propertyGroupStr in propertyGroupList)
                {
                    var propertyGroupXmlDoc = new XmlDocument();
                    propertyGroupXmlDoc.LoadXml(propertyGroupStr);
                    var propertyGroupXmlNode = propertyGroupXmlDoc.DocumentElement;
                    if (propertyGroupXmlNode?.Attributes == null) continue;
                    foreach (XmlAttribute attribute in propertyGroupXmlNode.Attributes)
                    {
                        if (!attribute.Name.Equals(CsprjFileDefinition.PropertyGroupAttributeName,
                            StringComparison.OrdinalIgnoreCase)) continue;
                        int outputPathStartIndex = propertyGroupStr.IndexOf(CsprjFileDefinition.OutputPathStartLabel,
                            StringComparison.OrdinalIgnoreCase);
                        int outputPathEndIndex = propertyGroupStr.IndexOf(CsprjFileDefinition.OutputPathEndLabel,
                            StringComparison.OrdinalIgnoreCase);
                        if (attribute.Value.IndexOf(CsprjFileDefinition.DebugCondition,
                                StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            selectedProject.DebugOutPath = propertyGroupStr
                                .Substring(outputPathStartIndex, outputPathEndIndex - outputPathStartIndex)
                                .Replace(CsprjFileDefinition.OutputPathStartLabel, "");
                        }
                        if (attribute.Value.IndexOf(CsprjFileDefinition.ReleaseCondition,
                                StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            selectedProject.ReleaseOutputPath = propertyGroupStr
                                .Substring(outputPathStartIndex, outputPathEndIndex - outputPathStartIndex)
                                .Replace(CsprjFileDefinition.OutputPathStartLabel, "");
                        }
                    }
                }
            }
            catch (Exception)
            {
                selectedProject.DebugOutPath = CsprjFileDefinition.DebugOutputDefaultPath;
                selectedProject.ReleaseOutputPath = CsprjFileDefinition.ReleaseOutputDefaultPath;
            }
        }

        public NugetInfo LoadNugetInfo(SelectedProjectInfo selectedProject)
        {
            NugetInfo nugetInfo = new NugetInfo();
            var nugetInfoXmlPath = string.Concat(selectedProject.ProjectDirPath, "\\", NugetInfoXmlDefinition.FileName);
            if (!File.Exists(nugetInfoXmlPath))
            {
                nugetInfoXmlPath = string.Concat(selectedProject.SolutionDirPath, "\\",
                    NugetInfoXmlDefinition.FileName);
            }
            if (!File.Exists(nugetInfoXmlPath))
            {
                return nugetInfo;
            }
            try
            {
                nugetInfo.IsLoadXml = true;
                var nugetInfoXmlDoc = new XmlDocument();
                nugetInfoXmlDoc.Load(nugetInfoXmlPath);

                var nugetInfoNode = nugetInfoXmlDoc.SelectSingleNode(NugetInfoXmlDefinition.RootNodeName);
                if (nugetInfoNode != null)
                {
                    var connStringNode = nugetInfoNode.SelectSingleNode(NugetInfoXmlDefinition.AuthKeyNodeName);
                    if (connStringNode != null)
                    {
                        nugetInfo.AuthKey = connStringNode.InnerText;
                    }
                    var templatesNodes = nugetInfoNode.SelectSingleNode(NugetInfoXmlDefinition.NugetSourceUrlNodeName);
                    if (templatesNodes != null)
                    {
                        nugetInfo.NugetSourceUrl = templatesNodes.InnerText;
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
            return nugetInfo;
        }
    }
}
