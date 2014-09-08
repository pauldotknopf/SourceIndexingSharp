using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SourceIndexingSharp.Indexing
{
    public interface IIndexProvider
    {
        string Name { get; }

        void WriteVariables(StreamWriter writer);

        void WriteSourceFiles(StreamWriter writer, List<string> sourceFiles);
    }
}
