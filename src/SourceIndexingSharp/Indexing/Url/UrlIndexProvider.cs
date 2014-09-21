using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using SourceIndexingSharp.Git;

namespace SourceIndexingSharp.Indexing.Url
{
    public class UrlIndexProvider : IIndexProvider
    {
        private readonly IStringExpander _stringExpander;
        private readonly string _gitDirectory;
        private readonly string _urlFormat;

        public UrlIndexProvider(IStringExpander stringExpander, string gitDirectory, string urlFormat)
        {
            _stringExpander = stringExpander;
            _gitDirectory = NormalizePath(gitDirectory);
            _urlFormat = urlFormat;
        }

        public string Name
        {
            get { return "Url"; }
        }

        public void WriteVariables(StreamWriter writer)
        {
            using (var repo = new GitRepo(_gitDirectory))
            {
                var variables = new Dictionary<string, string>();
                variables.Add("Commit", repo.Sha);
                variables.Add("File", "%var2%");
                var url = _stringExpander.Expand(_urlFormat, variables);

                writer.WriteLine("SRCSRVVERCTRL={0}", new Uri(url).Scheme);
                writer.WriteLine("SRCSRVTRG={0}", url);
            }
        }

        public void WriteSourceFiles(StreamWriter writer, List<string> sourceFiles)
        {
            using (var repo = new GitRepo(_gitDirectory))
            {
                foreach (var sourceFile in sourceFiles.Select(NormalizePath))
                {
                    if (!sourceFile.StartsWith(_gitDirectory))
                        continue;

                    var indexFile = sourceFile.Substring(_gitDirectory.Length + 1).ToLowerInvariant();

                    var repoFile = repo.Checksums.Keys.SingleOrDefault(x => string.Equals(x, indexFile, StringComparison.InvariantCultureIgnoreCase));

                    if (string.IsNullOrEmpty(repoFile))
                        continue;

                    writer.WriteLine("{0}*{1}", sourceFile, repoFile.Replace('\\', '/'));
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
