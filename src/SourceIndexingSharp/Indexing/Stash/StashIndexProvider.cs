using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using SourceIndexingSharp.Git;

namespace SourceIndexingSharp.Indexing.Stash
{
    public class StashIndexProvider : IIndexProvider
    {
        private readonly string _gitDirectory;
        private readonly string _host;
        private readonly string _project;
        private readonly string _repository;
        private readonly StashCredentials _credentials;
        
        public StashIndexProvider(string gitDirectory, string host, string project, string repository, StashCredentials credentials)
        {
            if(string.IsNullOrEmpty(gitDirectory))
                throw new Exception("You must provide the git directory the repository is located");

            if(string.IsNullOrEmpty(host))
                throw new Exception("You must provide a host");

            if(string.IsNullOrEmpty(project))
                throw new Exception("You must provide a project");

            if(string.IsNullOrEmpty(repository))
                throw new Exception("You must provide a repository");

            if(credentials == null)
                throw new Exception("You must provide credentials");

            _gitDirectory = NormalizePath(gitDirectory);
            _host = host;
            _project = project;
            _repository = repository;
            _credentials = credentials;
        }

        public string Name
        {
            get { return "Stash"; }
        }

        public void WriteVariables(StreamWriter writer)
        {
            var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version;
            writer.WriteLine("SRCSRVTRG=%targ%\\%var2%");
            using (var repo = new GitRepo(_gitDirectory))
            {
                writer.WriteLine(
                    "SRCSRVCMD={0} extractstash -version {1} -output %SRCSRVTRG% -host {2} -project {3} -repository {4} -file %var2% -commit {5} -username {6} -password {7}",
                    Context.ConsoleExePath(),
                    assemblyVersion,
                    _host,
                    _project,
                    _repository,
                    repo.Sha,
                    _credentials.UserName,
                    _credentials.Password);
            }
        }

        public void WriteSourceFiles(StreamWriter writer, List<string> sourceFiles)
        {
            using (var repo = new GitRepo(_gitDirectory))
            {
                foreach (var sourceFile in sourceFiles.Select(NormalizePath))
                {
                    if(!sourceFile.StartsWith(_gitDirectory))
                        continue;

                    var indexFile = sourceFile.Substring(_gitDirectory.Length + 1).ToLowerInvariant();

                    var repoFile = repo.Checksums.Keys.SingleOrDefault(x => string.Equals(x, indexFile, StringComparison.InvariantCultureIgnoreCase));

                    if(string.IsNullOrEmpty(repoFile))
                        continue;

                    writer.WriteLine("{0}*{1}", sourceFile, repoFile);
                }
            }
        }

        private string NormalizePath(string path)
        {
            return Path.GetFullPath(new Uri(path).LocalPath)
                       .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                       .ToLowerInvariant();
        }
    }
}
