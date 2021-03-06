﻿using System;
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
    public class IndexProviderTests : TestBase
    {
        [Test]
        public void Can_index_and_extract_source_files()
        {
            // arrange
            var pdbFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SourceIndexingSharp.pdb");

            // act
            _indexer.IndexFile(pdbFile, new SourceIndexingPdbProviderTests());

            // assert
            var output = _srcTool.Dump(pdbFile, SrcToolFlags.ExtractFiles | SrcToolFlags.ShowVersionControlCommandsWhileExtracting, dumpDirectory: _extractionDirectory);
            Assert.That(output, Is.StringEnding("source files were extracted."));
            Assert.That(Directory.GetFiles(_extractionDirectory), Has.Length.GreaterThan(0));
        }

        class SourceIndexingPdbProviderTests : IIndexProvider
        {
            public string Name
            {
                get { return GetType().Name; }
            }

            public void WriteVariables(StreamWriter writer)
            {
                writer.WriteLine("SRCSRVTRG=%targ%\\%var2%");
                writer.WriteLine("SRCSRVCMD=cmd /c copy \"%var1%\" \"%SRCSRVTRG%\"");
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
