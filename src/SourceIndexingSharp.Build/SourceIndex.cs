using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
                VerifyConfigFile();

                return true;
            }
            catch (Exception ex)
            {
                Log.LogError(ex.Message);
                return false;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Resolve the config path and verify it exists.
        /// Throws an exception if the config file is not valid.
        /// </summary>
        private void VerifyConfigFile()
        {
            if (string.IsNullOrEmpty(_configFile))
                throw new Exception("The 'ConfigFile' attribute must be provided.");

            if (Path.IsPathRooted(_configFile))
            {
                if (!File.Exists(_configFile))
                    throw new Exception(string.Format("No configuration file exists at the path '{0}'.", _configFile));
            }
            else
            {
                var projDirectory = new FileInfo(this.BuildEngine3.ProjectFileOfTaskNode).Directory.FullName;

                _configFile = Path.GetFullPath(Path.Combine(projDirectory, _configFile));

                if (!File.Exists(_configFile))
                    throw new Exception(string.Format("No configuration file exists at the path '{0}'.", _configFile));
            }
        }

        #endregion
    }
}
