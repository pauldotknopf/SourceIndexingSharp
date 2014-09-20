using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SourceIndexingSharp.Indexing.Stash;

namespace SourceIndexingSharp
{
    public interface IPdbCommandProcessor
    {
        void Process(List<string> pdbFiles, string command, string relativeDirectory);

        void ProcessStash(List<string> pdbFiles, string gitDirectory, string host, string project, string repository, string username, string password);

        void ProcessUrl(List<string> pdbFiles, string gitDirectory, string urlFormat);
    }
}
