using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using NUnit.Framework;
using SourceIndexingSharp.Build;

namespace SourceIndexingSharp.Tests
{
    [TestFixture]
    public class SourceIndexTaskTests : TestBase
    {
        private string _testDirectory;
        private string _projPath;

        [Test]
        public void Can_resolve_path_relative()
        {
            // arrange
            var errors = new List<string>();
            var messages = new List<string>();
            var logger = new Logger();
            logger.ErrorRaised += (sender, args) => errors.Add(args.Message);
            logger.MessageRaised += (sender, args) => messages.Add(args.Message);
            SetupProjWithConfigPath("config.json");
            var project = _buildInformation.Load(_projPath, new Dictionary<string, string>());
            File.WriteAllText(Path.Combine(new FileInfo(_projPath).Directory.FullName, "config.json"), "");

            // act
            var result = project.Build(logger);

            // assert
            Assert.That(errors, Is.Empty);
            Assert.That(result, Is.True);
        }

        [Test]
        public void Error_raised_if_config_path_doesnt_exist()
        {
            // arrange
            var errors = new List<string>();
            var messages = new List<string>();
            var logger = new Logger();
            logger.ErrorRaised += (sender, args) => errors.Add(args.Message);
            logger.MessageRaised += (sender, args) => messages.Add(args.Message);
            SetupProjWithConfigPath("config.json");
            var project = _buildInformation.Load(_projPath, new Dictionary<string, string>());

            // act
            var result = project.Build(logger);

            // assert
            Assert.That(result, Is.False);
            Assert.That(errors, Has.Count.EqualTo(1));
            Assert.That(errors[0], Is.StringStarting("SourceIndex: No configuration file exists at the path"));
        }

        private void SetupProjWithConfigPath(string path)
        {
             if(File.Exists(_projPath))
                File.Delete(_projPath);

            File.WriteAllText(_projPath, string.Format(
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                "<Project ToolsVersion=\"4.0\" DefaultTargets=\"Build\" xmlns=\"http://schemas.microsoft.com/developer/msbuild/2003\">" +
                "  <UsingTask TaskName=\"SourceIndexingSharp.Build.SourceIndex\" AssemblyFile=\"..\\SourceIndexingSharp.Build.dll\" />" +
                "  <Target Name=\"Build\">" +
                "      <SourceIndexingSharp.Build.SourceIndex ConfigFile=\"" + path + "\" PdbFiles=\"..\\SourceIndexingSharp.Build.pdb\" />" +
                "  </Target>" +
                "</Project>"));
        }

        public override void Setup()
        {
            base.Setup();
            _testDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestDirectory");
            if (!Directory.Exists(_testDirectory))
                Directory.CreateDirectory(_testDirectory);
            _projPath = Path.Combine(_testDirectory, "test.proj");
        }

        public override void TearDown()
        {
            base.TearDown();
            if(Directory.Exists(_testDirectory))
                Directory.Delete(_testDirectory, true);
            if(File.Exists(_projPath))
                File.Delete(_projPath);
        }
    }
}
