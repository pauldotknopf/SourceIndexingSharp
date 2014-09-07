using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SourceIndexingSharp.Stash
{
    public interface IStashApi
    {
        void ExtractSource(string destination, string host, string project, string repository, string file, string commit, StashCredentials credentials);
    }
}
