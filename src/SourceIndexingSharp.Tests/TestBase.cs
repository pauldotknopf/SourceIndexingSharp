using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SourceIndexingSharp.Build;
using SourceIndexingSharp.Indexing;
using SourceIndexingSharp.Tools;

namespace SourceIndexingSharp.Tests
{
    public class TestBase
    {
        // ReSharper disable InconsistentNaming
        protected Paths _paths;
        protected IIndexer _indexer;
        protected ISrcTool _srcTool;
        protected IPdbReaderWriter _pdbReaderWriter;
        protected IBuildInformation _buildInformation;
        protected IStringExpander _stringExpander;
        protected string _extractionDirectory;
        // ReSharper restore InconsistentNaming

        [SetUp]
        public virtual void Setup()
        {
            Context.ExtractorExe = () => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SourceIndexingSharpExtractor.exe");
            _paths = new Paths
            {
                SrcToolPath = @"C:\Program Files (x86)\Windows Kits\8.1\Debuggers\x86\srcsrv\srctool.exe",
                PdbStrPath = @"C:\Program Files (x86)\Windows Kits\8.1\Debuggers\x86\srcsrv\pdbstr.exe"
            };
            _srcTool = new SrcTool(_paths);
            _pdbReaderWriter = new PdbReaderWriter(_paths);
            _indexer = new Indexer(_pdbReaderWriter, _srcTool);
            _buildInformation = new BuildInformation();
            _stringExpander = new StringExpander();
            _extractionDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetRandomFileName());
            if (!Directory.Exists(_extractionDirectory))
                Directory.CreateDirectory(_extractionDirectory);
        }
        
        [TearDown]
        public virtual void TearDown()
        {
            if (Directory.Exists(_extractionDirectory))
                Directory.Delete(_extractionDirectory, true);
        }
    }
}
