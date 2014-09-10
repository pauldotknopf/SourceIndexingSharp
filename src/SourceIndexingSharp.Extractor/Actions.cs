using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PowerArgs;
using SourceIndexingSharp.Git;

namespace SourceIndexingSharp.Extractor
{
    public class Actions
    {
        [ArgActionMethod]
        public void Test(TestArgs args)
        {
            ValidateVersion(args);

            if(string.IsNullOrEmpty(args.OutputFileLocation))
                throw new Exception("You must specify a file location");

            if(File.Exists(args.OutputFileLocation))
                File.Delete(args.OutputFileLocation);

            File.WriteAllText(args.OutputFileLocation, "TESTFILE: " + args.OutputFileLocation.ToLower());
        }

        private void ValidateVersion(BaseArgs args)
        {
            var actionsAssemlly = typeof (Actions).Assembly;
            var currentVersion = actionsAssemlly.GetName().Version;
            Console.WriteLine("Args version = null ? " + (args.Version == null));
            Console.WriteLine("Args == null ? " + (args == null));
            Console.WriteLine("Current version == null ? " + (currentVersion == null));
            Console.WriteLine("currentVersion:" + currentVersion);
            Console.WriteLine("version: " + args.Version);
            if(currentVersion < args.Version)
                throw new SourceIndexException(string.Format("You asking the extractor to run at version {0}, but it was compiled at {1}.", args.Version, currentVersion));
        }

        [ArgReviver]
        public static Version Revive(string key, string value)
        {
            Version result = null;

            if(!Version.TryParse(value, out result))
                throw new ArgException(string.Format("Invalid version format \"{0}\"", value));

            return result;
        }

        public class TestArgs : BaseArgs
        {
            public string OutputFileLocation { get; set; }
        }

        public class BaseArgs
        {
            [ArgDescription("The version that the PDB file ")]
            [ArgRequired]
            public Version Version { get; set; }
        }
    }
}
