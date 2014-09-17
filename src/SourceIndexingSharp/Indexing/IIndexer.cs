using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SourceIndexingSharp.Indexing
{
    public interface IIndexer
    {
        void IndexFiles(IEnumerable<string> files, IIndexProvider indexProvider);
        void IndexFile(string file, IIndexProvider indexProvider);
    }
}
