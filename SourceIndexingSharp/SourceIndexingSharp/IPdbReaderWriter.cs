using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SourceIndexingSharp
{
    public interface IPdbReaderWriter
    {
        void WritePdb(string pdbFile, Action<StreamWriter> writerAction, string streamName = "srcsrv");

        string ReadPdb(string pdbFile, string streamName = "srcsrv");
    }
}
