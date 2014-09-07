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
    public class SrcToolTests : TestBase
    {
        [Test]
        public void Can_dump_raw_files_from_pdb()
        {
            // arrange
            var pdbFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "SourceIndexingSharp.pdb");

            // act
            var dump = _srcTool.Dump(pdbFile);

            // assert
            Assert.That(dump, Is.Not.Null.Or.Empty);
            Assert.That(dump, Is.StringStarting("No source information in pdb"));
        }
        
        [Test]
        public void Can_get_indexed_source_files()
        {
            // arrange
            var pdbFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SourceIndexingSharp.pdb");

            // act
            var files = _srcTool.GetSourceFiles(pdbFile);

            // assert
            Assert.That(files, Has.Count.GreaterThan(0));
        }
    }
}
