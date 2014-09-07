using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SourceIndexingSharp.Indexing
{
    public interface IIndexer
    {
        void IndexFile(string file, IIndexProvider indexProvider);
    }
}
