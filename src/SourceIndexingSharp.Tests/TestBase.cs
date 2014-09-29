using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SourceIndexingSharp.Build;
using SourceIndexingSharp.Indexing;
using SourceIndexingSharp.Tools;

namespace SourceIndexingSharp.Tests
{
    public class TestBase
    {
        // ReSharper disable InconsistentNaming
        protected string _extractionDirectory;
        protected IBuildInformation _buildInformation;
        // ReSharper restore InconsistentNaming

        [SetUp]
        public virtual void Setup()
        {
            Context.ConsoleExePath = () => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SourceIndexingSharpCons.exe");
            _extractionDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Path.GetRandomFileName());
            _buildInformation = new BuildInformation();
            if (!Directory.Exists(_extractionDirectory))
                Directory.CreateDirectory(_extractionDirectory);
        }
        
        [TearDown]
        public virtual void TearDown()
        {
            if (Directory.Exists(_extractionDirectory))
                Directory.Delete(_extractionDirectory, true);
        }
    }
}
