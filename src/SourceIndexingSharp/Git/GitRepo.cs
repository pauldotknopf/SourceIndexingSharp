using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using LibGit2Sharp;

namespace SourceIndexingSharp.Git
{
    public class GitRepo : IDisposable
    {
        readonly Repository _repository;
        readonly Dictionary<string, string> _checksums = new Dictionary<string, string>();

        public GitRepo(string directory)
        {
            GitLibInitializer.EnsureInitialized();

            _repository = new Repository(directory);
            foreach (var file in _repository.Index)
                _checksums.Add(file.Path, file.Id.Sha);
        }

        public string Sha { get { return _repository.Head.Tip.Sha; } }

        public Dictionary<string, string> Checksums { get { return _checksums; } }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
