using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SourceIndexingSharp.Tools;

namespace SourceIndexingSharp.Tests
{
    [TestFixture]
    public class ExtractingTests : TestBase
    {
        ISrcTool _srcTool;
        IPdbReaderWriter _pdbReaderWriter;
        private string _extractionDirectory;

        [Test]
        public void Can_index_and_extract_source_files()
        {
            // arrange
            var pdbFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SourceIndexingSharp.pdb");
            var sourceFiles = _srcTool.GetSourceFiles(pdbFile);
            _pdbReaderWriter.WritePdb(pdbFile, writer =>
            {
                writer.WriteLine("SRCSRV: ini ------------------------------------------------");
                writer.WriteLine("VERSION=1");
                writer.WriteLine("VERCTRL=TESTCOPY");
                writer.WriteLine("DATETIME=" + DateTime.UtcNow.ToString("u"));
                writer.WriteLine("SRCSRV: variables ------------------------------------------");
                writer.WriteLine("SRCSRVTRG=%targ%\\%var2%");
                writer.WriteLine("SRCSRVCMD=cmd /c copy \"%var1%\" \"%SRCSRVTRG%\"");
                writer.WriteLine("SRCSRV: source files ---------------------------------------");
                foreach (var sourceFile in sourceFiles)
                {
                    var startingPathIndex = sourceFile.LastIndexOf("SourceIndexingSharp", StringComparison.Ordinal) + "SourceIndexingSharp".Length + 1;
                    writer.WriteLine("{0}*{1}", sourceFile, sourceFile.Substring(startingPathIndex));
                }
                writer.WriteLine("SRCSRV: end ------------------------------------------------");
            });

            // act
            var output = _srcTool.Dump(pdbFile, SrcToolFlags.ExtractFiles, dumpDirectory:_extractionDirectory);

            // assert
            Assert.That(output, Is.StringEnding("source files were extracted."));
            Assert.That(Directory.GetFiles(_extractionDirectory), Has.Length.GreaterThan(0));
        }

        public override void Setup()
        {
            base.Setup();
            _srcTool = new SrcTool(_paths);
            _pdbReaderWriter = new PdbReaderWriter(_paths);
            _extractionDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetRandomFileName());
            if (!Directory.Exists(_extractionDirectory))
                Directory.CreateDirectory(_extractionDirectory);
        }

        public override void TearDown()
        {
            base.TearDown();
            if(Directory.Exists(_extractionDirectory))
                Directory.Delete(_extractionDirectory, true);
        }
    }
}
