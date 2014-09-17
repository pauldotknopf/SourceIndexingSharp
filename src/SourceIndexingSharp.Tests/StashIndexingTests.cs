using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SourceIndexingSharp.Indexing.Stash;
using SourceIndexingSharp.Tools;

namespace SourceIndexingSharp.Tests
{
    [TestFixture]
    public class StashIndexingTests : TestBase
    {
        IStashApi _stashApi;

        [Test, Ignore]
        public void Can_retrieve_raw_source_file_from_http()
        {
            // arrange
            var destination = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App.xaml");
            if (File.Exists(destination))
                File.Delete(destination);

            // act
            _stashApi.ExtractSource(destination, "stash.medxchange.com", "EVO", "Software", "Src/MedXChange.Evo.Presentation/App.xaml", "5fadaa3dd87b817863253745ae67b1a29efbb1ca", new StashCredentials("pknopf", "youwish!"));

            // assert
            Assert.That(File.Exists(destination));
            Assert.That(File.ReadAllText(destination), Is.StringStarting("<?xml version=\"1.0\" encoding=\"utf-8\"?>"));
        }

        [Test, Ignore]
        public void Can_extract_stash_sources()
        {
            // arrange
            var extractorLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"SourceIndexingSharpExtractor.exe");
            extractorLocation = "SourceIndexingSharpExtractor.exe";
            var pdbFile = @"E:\Git\testpdb\ErrorApp\ErrorApp\bin\Release\ErrorApp.pdb";
            var indexProvider = new StashIndexProvider(extractorLocation, @"E:\Git\testpdb", "stash.medxchange.com", "SB", "TestPDB", new StashCredentials("pknopf", "1234qwer"));
            _indexer.IndexFile(pdbFile, indexProvider);

            // act
            var output = _srcTool.Dump(pdbFile, SrcToolFlags.ExtractFiles | SrcToolFlags.ShowVersionControlCommandsWhileExtracting, dumpDirectory: _extractionDirectory);
            Assert.That(output, Is.StringEnding("source files were extracted."));
            Assert.That(Directory.GetFiles(_extractionDirectory), Has.Length.GreaterThan(0));
        }

        public override void Setup()
        {
            base.Setup();
            _stashApi = new StashApi();
        }
    }
}
