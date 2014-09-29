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
        [Test, Ignore]
        public void Can_retrieve_raw_source_file_from_http()
        {
            // arrange
            var destination = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App.xaml");
            if (File.Exists(destination))
                File.Delete(destination);

            // act
            Context.StashApi.ExtractSource(destination, "stash.medxchange.com", "EVO", "Software", "Src/MedXChange.Evo.Presentation/App.xaml", "5fadaa3dd87b817863253745ae67b1a29efbb1ca", new StashCredentials("pknopf", "youwish!"));

            // assert
            Assert.That(File.Exists(destination));
            Assert.That(File.ReadAllText(destination), Is.StringStarting("<?xml version=\"1.0\" encoding=\"utf-8\"?>"));
        }

        [Test, Ignore]
        public void Can_extract_stash_sources()
        {
            // arrange
            const string pdbFile = @"E:\Git\testpdb\ErrorApp\ErrorApp\bin\Release\ErrorApp.pdb";
            Context.CommandProcessor.ProcessCommand("index -provider stash -gitdirectory {0} -host {1} -project {2} -repository {3} -username {4} -password {5}");
            //Context.PdbCommandProcessor.Process(new List<string>{pdbFile},
            //    "{" +
            //    "   Provider: \"Stash\"," +
            //    "   GitDirectory: \"E:\\\\Git\\\\testpdb\"," +
            //    "   Host: \"stash.medxchange.com\"," +
            //    "   Project: \"SB\"," +
            //    "   Repository: \"TestPDB\"," +
            //    "   Username: \"pknopf\"," +
            //    "   Password: \"youwish!\"" + 
            //    "}",
            //    null);

            // act
            var output = Context.SrcTool.Dump(pdbFile, SrcToolFlags.ExtractFiles | SrcToolFlags.ShowVersionControlCommandsWhileExtracting, dumpDirectory: _extractionDirectory);
            Assert.That(output, Is.StringEnding("source files were extracted."), output);
        }
    }
}
