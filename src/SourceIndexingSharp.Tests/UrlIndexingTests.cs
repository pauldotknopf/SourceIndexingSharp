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
    public class UrlIndexingTests : TestBase
    {
        [Test]
        public void Can_extract_url_files()
        {
            // arrange
            var pdbFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SourceIndexingSharp.pdb");
            Context.PdbCommandProcessor.Process(new List<string> { pdbFile },
                "{" +
                "   Provider: \"Url\"," +
                "   GitDirectory: \"E:\\\\Git\\\\SourceIndexingSharp\"," +
                "   UrlFormat: \"https://raw.githubusercontent.com/theonlylawislove/SourceIndexingSharp/{{Commit}}/{{File}}\"," +
                "}",
                null);

            // act
            var output = Context.SrcTool.Dump(pdbFile, SrcToolFlags.ExtractFiles | SrcToolFlags.ShowVersionControlCommandsWhileExtracting, dumpDirectory: _extractionDirectory);
            Assert.That(output, Is.StringEnding("source files were extracted."), output);
        }
    }
}
