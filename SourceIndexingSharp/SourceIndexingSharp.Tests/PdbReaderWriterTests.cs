using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace SourceIndexingSharp.Tests
{
    [TestFixture]
    public class PdbReaderWriterTests : TestBase
    {
        private IPdbReaderWriter _pdbReaderWriter;

        [Test]
        public void Can_read_write_pdb_files()
        {
            // arrange
            var pdbFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "SourceIndexingSharp.pdb");

            // act
            _pdbReaderWriter.WritePdb(pdbFile, writer => writer.Write("Test line..."));

            // assert
            Assert.That(_pdbReaderWriter.ReadPdb(pdbFile), Is.EqualTo("Test line..."));
        }

        public override void Setup()
        {
            base.Setup();
            _pdbReaderWriter = new PdbReaderWriter(_tools);
        }
    }
}
