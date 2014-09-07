using System;
using System.IO;

namespace SourceIndexingSharp.Tools
{
    public interface IPdbReaderWriter
    {
        void WritePdb(string pdbFile, Action<StreamWriter> writerAction, string streamName = "srcsrv");

        string ReadPdb(string pdbFile, string streamName = "srcsrv");
    }
}
