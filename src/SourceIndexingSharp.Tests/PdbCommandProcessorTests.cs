using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Moq;
using NUnit.Framework;

namespace SourceIndexingSharp.Tests
{
    [TestFixture]
    public class PdbCommandProcessorTests
    {
        private Mock<PdbCommandProcessor> _pdbCommandProcessor;

        [Test]
        public void Unknown_command_throws_exception()
        {
            Assert.Throws<Exception>(() =>
                    _pdbCommandProcessor.Object.Process(
                        new List<string>
                        {
                            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SourceIndexingSharp.pdb")
                        },
                        "{Provider:\"Unknown\"}",
                        null));
        }

        [Test]
        public void Can_parse_command_for_stash()
        {
            // arrange
            _pdbCommandProcessor.Setup(x => x.ProcessStash(It.IsAny<List<string>>(),
                        new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName,
                        "stash.test.com",
                        "testproject",
                        "testrepo",
                        "testuser",
                        "testpassword"));

            // act
            _pdbCommandProcessor.Object.Process(new List<string>
                        {
                            "SourceIndexingSharp.pdb"
                        },
                        "{" +
                        "   Provider: \"Stash\"," +
                        "   GitDirectory: \"..\"," +
                        "   Host: \"stash.test.com\"," +
                        "   Project: \"testproject\"," +
                        "   Repository: \"testrepo\"," +
                        "   Username: \"testuser\"," +
                        "   Password: \"testpassword\"" + 
                        "}",
                        null);

            // assert
            _pdbCommandProcessor.VerifyAll();
        }

        [Test]
        public void Can_parse_command_for_url()
        {
            // arrange
            _pdbCommandProcessor.Setup(x => x.ProcessUrl(It.IsAny<List<string>>(),
                        new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.FullName,
                        "http://testformat.com/"));

            // act
            _pdbCommandProcessor.Object.Process(new List<string>
                        {
                            "SourceIndexingSharp.pdb"
                        },
                        "{" +
                        "   Provider: \"Url\"," +
                        "   GitDirectory: \"..\"," +
                        "   UrlFormat: \"http://testformat.com/\"" +
                        "}",
                        null);

            // assert
            _pdbCommandProcessor.VerifyAll();
        }

        [SetUp]
        public void Setup()
        {
            _pdbCommandProcessor = new Mock<PdbCommandProcessor>(new StringExpander(), new PathResolver());
            _pdbCommandProcessor.CallBase = true;
        }
    }
}
