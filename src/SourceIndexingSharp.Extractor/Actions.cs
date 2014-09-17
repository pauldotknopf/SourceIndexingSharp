using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PowerArgs;
using SourceIndexingSharp.Git;
using SourceIndexingSharp.Indexing.Stash;

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

        [ArgActionMethod]
        public void ExtractStash(StashArgs args)
        {
            ValidateVersion(args);

            Context.StashApi.ExtractSource(args.Output, args.Host, args.Project, args.Repository, args.File, args.Commit, new StashCredentials(args.Username, args.Password));

            if(!File.Exists(args.Output))
                throw new Exception("Couldn't extract source to " + args.Output);
        }

        private void ValidateVersion(BaseArgs args)
        {
            var actionsAssemlly = typeof (Actions).Assembly;
            var currentVersion = actionsAssemlly.GetName().Version;
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

        public class StashArgs : BaseArgs
        {
            public string Output { get; set; }

            public string Host { get; set; }

            public string Project { get; set; }

            public string Repository { get; set; }

            public string File { get; set; }

            public string Commit { get; set; }

            public string Username { get; set; }

            public string Password { get; set; }
        }

        public class BaseArgs
        {
            [ArgRequired]
            public Version Version { get; set; }
        }
    }
}
