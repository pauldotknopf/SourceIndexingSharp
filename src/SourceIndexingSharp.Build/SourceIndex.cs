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
                _rootDirectory = new FileInfo(BuildEngine3.ProjectFileOfTaskNode).Directory.FullName;

                _configFile = Context.PathResolver.ResolvePath(_configFile, _rootDirectory);

                if (!File.Exists(_configFile))
                    throw new Exception(string.Format("No configuration file exists at the path '{0}'.", _configFile));

                var pdbFiles = PdbFiles.Split(';').Select(x =>
                {
                    var pdbPath = Context.PathResolver.ResolvePath(x, _rootDirectory);
                    if(!File.Exists(pdbPath))
                        throw new Exception(string.Format("The file '{0}' could not be found.", pdbPath));
                    return pdbPath;
                }).ToList();

                Context.PdbCommandProcessor.Process(pdbFiles, File.ReadAllText(_configFile), _rootDirectory);

                return true;
            }
            catch (Exception ex)
            {
                Log.LogError("SourceIndex: " + ex.Message);
                return false;
            }
        }

        #endregion
    }
}
