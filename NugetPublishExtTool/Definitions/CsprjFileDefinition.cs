using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NugetPublishExtTool.Definitions
{
    public class CsprjFileDefinition
    {
        public const string CsprjXmlNameSpaceAliasName = "ns";
        public const string PropertyGroupAttributeName = "Condition";
        public static string PropertyGroupXpath = $"./{CsprjXmlNameSpaceAliasName}:PropertyGroup[@{PropertyGroupAttributeName}]";
        public const string DebugCondition = "debug";
        public const string ReleaseCondition = "release";
        public const string OutputPathStartLabel = "<OutputPath>";
        public const string OutputPathEndLabel = "</OutputPath>";

        public const string DebugOutputDefaultPath = "bin\\Debug\\";
        public const string ReleaseOutputDefaultPath = "bin\\Release\\";
    }
}
