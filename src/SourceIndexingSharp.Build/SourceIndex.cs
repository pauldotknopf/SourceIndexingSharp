using System;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace SourceIndexingSharp.Build
{
    /// <summary>
    /// Index the pdb files
    /// </summary>
    public class SourceIndex : Task
    {
        #region Fields

        private string _configFile;
        private string _pdbFiles;
        private string _rootDirectory;

        #endregion

        #region Properties

        /// <summary>
        /// The location where the configuration file for source indexing is located.
        /// </summary>
        [Required]
        public string ConfigFile
        {
            get { return _configFile; }
            set { _configFile = value; }
        }

        /// <summary>
        /// The PDB files to index
        /// </summary>
        [Required]
        public string PdbFiles
        {
            get { return _pdbFiles; }
            set { _pdbFiles = value; }
        }

        #endregion

        #region Task

        /// <summary>
        /// When overridden in a derived class, executes the task.
        /// </summary>
        /// <returns>
        /// true if the task successfully executed; otherwise, false.
        /// </returns>
        public override bool Execute()
        {
            try
            {
                _rootDirectory = new FileInfo(this.BuildEngine3.ProjectFileOfTaskNode).Directory.FullName;

                VerifyConfigFile();

                var pdbFiles = PdbFiles.Split(';').Select(ResolveFilePath).ToList();

                return true;
            }
            catch (Exception ex)
            {
                Log.LogError("SourceIndex: " + ex.Message);
                return false;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Resolve a possible relative path, and throw an exception if the file doesn't exist.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private string ResolveFilePath(string path)
        {
            if (Path.IsPathRooted(path))
            {
                if (!File.Exists(_configFile))
                    throw new Exception(string.Format("The file '{0}' could not be found.", path));
                return path;
            }

            var possiblePath = Path.GetFullPath(Path.Combine(_rootDirectory, path));

            if (!File.Exists(possiblePath))
                throw new Exception(string.Format("The file '{0}' could not be found.", possiblePath));

            return possiblePath;
        }

        /// <summary>
        /// Resolve the config path and verify it exists.
        /// Throws an exception if the config file is not valid.
        /// </summary>
        private void VerifyConfigFile()
        {
            if (Path.IsPathRooted(_configFile))
            {
                if (!File.Exists(_configFile))
                    throw new Exception(string.Format("No configuration file exists at the path '{0}'.", _configFile));
            }
            else
            {
                _configFile = Path.GetFullPath(Path.Combine(_rootDirectory, _configFile));

                if (!File.Exists(_configFile))
                    throw new Exception(string.Format("No configuration file exists at the path '{0}'.", _configFile));
            }
        }

        #endregion
    }
}
