using System;
using System.Collections.Generic;
using System.IO;
using EnvDTE;

namespace NugetPublishExtTool.Models
{
    public class SelectedProjectInfo
    {
        private readonly string _projectFileFullPath;
        private readonly string _projectName;
        private readonly string _projectDirPath;
        private readonly Project _projectDte;
        private readonly List<string> _csFilesName;

        private string _releaseOutPath;
        private string _debugOutPath;

        private readonly Solution _solutionDte;
        private readonly string _solutionDirPath;
        private readonly string _solutionFileFullPath;

        public SelectedProjectInfo(string projectFileFullPath, Project projectDte,List<string> csFilesName, Solution solutionDte)
        {
            _projectFileFullPath = projectFileFullPath;
            _projectDte = projectDte;
            _csFilesName = csFilesName;
            _projectName = Path.GetFileNameWithoutExtension(ProjectFileFullPath);
            _projectDirPath = Path.GetDirectoryName(ProjectFileFullPath);
            _solutionDte = solutionDte;
            _solutionFileFullPath = _solutionDte.FullName;
            _solutionDirPath = Path.GetDirectoryName(_solutionFileFullPath);
        }

        public string ProjectFileFullPath => _projectFileFullPath;

        public string ProjectName => _projectName;

        public string ProjectDirPath => _projectDirPath;

        public Project ProjectDte => _projectDte;

        public Solution SolutionDte => _solutionDte;

        public List<string> CsFilesName => _csFilesName;

        public string DebugOutPath { get { return _debugOutPath; } set { _debugOutPath = value; } }

        public string ReleaseOutputPath { get { return _releaseOutPath; } set { _releaseOutPath = value; } }

        public string DebugOutputFullPath => ProjectFileFullPath == null ? "" : String.Concat(_projectDirPath, "\\", _debugOutPath);

        public string ReleaseOutputFullPath => ProjectFileFullPath == null ? "" : String.Concat(_projectDirPath, "\\", _releaseOutPath);

        public string SolutionFileFullPath => _solutionFileFullPath;

        public string SolutionDirPath => _solutionDirPath;
    }
}
