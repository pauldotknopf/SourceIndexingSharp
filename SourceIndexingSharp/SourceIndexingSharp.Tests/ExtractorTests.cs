using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SourceIndexingSharp.Indexing;
using SourceIndexingSharp.Tools;

namespace SourceIndexingSharp.Tests
{
    [TestFixture]
    public class ExtractorTests : TestBase
    {
        [Test]
        public void Can_invoke_extractor()
        {
            // arrange
            var pdbFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SourceIndexingSharp.pdb");
            var extractorLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"SourceIndexingSharpExtractor.exe");

            // act
            _indexer.IndexFile(pdbFile, new ExtractorProviderTests(extractorLocation));

            // assert
            var output = _srcTool.Dump(pdbFile, SrcToolFlags.ExtractFiles | SrcToolFlags.ShowVersionControlCommandsWhileExtracting, dumpDirectory: _extractionDirectory);
            Assert.That(output, Is.StringEnding("source files were extracted."));
            Assert.That(Directory.GetFiles(_extractionDirectory), Has.Length.GreaterThan(0));
            foreach (var file in Directory.GetFiles(_extractionDirectory))
                Assert.That(File.ReadAllText(file), Is.EqualTo("TESTFILE: " + file.ToLower()));
        }

        class ExtractorProviderTests : IIndexProvider
        {
            private readonly string _extractorLocation;

            public ExtractorProviderTests(string extractorLocation)
            {
                _extractorLocation = extractorLocation;
            }

            public string Name
            {
                get { return GetType().Name; }
            }

            public void WriteVariables(StreamWriter writer)
            {
                writer.WriteLine("SRCSRVTRG=%targ%\\%var2%");
                writer.WriteLine("SRCSRVCMD={0} test -o \"%SRCSRVTRG%\"", _extractorLocation);
            }

            public void WriteSourceFiles(StreamWriter writer, List<string> sourceFiles)
            {
                foreach (var sourceFile in sourceFiles)
                {
                    var startingPathIndex = sourceFile.LastIndexOf("SourceIndexingSharp", StringComparison.Ordinal) + "SourceIndexingSharp".Length + 1;
                    writer.WriteLine("{0}*{1}", sourceFile, sourceFile.Substring(startingPathIndex));
                }
            }
        }
    }
}
