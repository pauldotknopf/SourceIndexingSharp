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
    public class PdbReaderWriterTests : TestBase
    {
        [Test]
        public void Can_read_write_pdb_files()
        {
            // arrange
            var pdbFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SourceIndexingSharp.pdb");
            
            // act
            Context.PdbReaderWriter.WritePdb(pdbFile, writer => writer.Write("Test line..."));

            // assert
            Assert.That(Context.PdbReaderWriter.ReadPdb(pdbFile), Is.EqualTo("Test line..."));
        }
    }
}
