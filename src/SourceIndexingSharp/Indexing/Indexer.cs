using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using SourceIndexingSharp.Tools;

namespace SourceIndexingSharp.Indexing
{
    public class Indexer : IIndexer
    {
        private readonly IPdbReaderWriter _pdbReaderWriter;
        private readonly ISrcTool _srcTool;

        public Indexer(IPdbReaderWriter pdbReaderWriter, ISrcTool srcTool)
        {
            _pdbReaderWriter = pdbReaderWriter;
            _srcTool = srcTool;
        }

        public void IndexFiles(IEnumerable<string> files, IIndexProvider indexProvider)
        {
            foreach (var file in files)
                IndexFile(file, indexProvider);
        }

        public void IndexFile(string file, IIndexProvider indexProvider)
        {
            var providerName = indexProvider.Name;

            if(string.IsNullOrEmpty(providerName))
                throw new SourceIndexException("The index provider must have a name");

            if (!File.Exists(file))
                throw new SourceIndexException("File doesn't exist. " + file);

            var sourceFiles = _srcTool.GetSourceFiles(file);

            _pdbReaderWriter.WritePdb(file, writer =>
            {
                writer.WriteLine("SRCSRV: ini ------------------------------------------------");
                writer.WriteLine("VERSION=1");
                writer.WriteLine("VERCTRL=" + providerName);
                writer.WriteLine("DATETIME=" + DateTime.UtcNow.ToString("u"));
                writer.WriteLine("SRCSRV: variables ------------------------------------------");
                indexProvider.WriteVariables(writer);
                writer.WriteLine("SRCSRV: source files ---------------------------------------");
                indexProvider.WriteSourceFiles(writer, sourceFiles);
                writer.WriteLine("SRCSRV: end ------------------------------------------------");
            });
        }
    }
}
