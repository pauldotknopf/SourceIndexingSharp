using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SourceIndexingSharp.Build;

namespace SourceIndexingSharp.Tests
{
    [TestFixture]
    public class BuildInformationTests : TestBase
    {
        IBuildInformation _buildInformation;

        [Test]
        public void Can_get_compile_includes()
        {
            // arrange
            var projectFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestData", "SampleProject", "SampleProject.csproj");
            var project = _buildInformation.Load(projectFile, new Dictionary<string, string>());

            // act
            var compiledItems = _buildInformation.GetCompileItems(project);

            // assert
            Assert.That(compiledItems, Has.Count.EqualTo(3));
            Assert.That(compiledItems[0], Is.EqualTo(@"Properties\AssemblyInfo.cs"));
            Assert.That(compiledItems[1], Is.EqualTo(@"TestClass1.cs"));
            Assert.That(compiledItems[2], Is.EqualTo(@"TestClass2\TestClass2.cs"));
        }

        public override void Setup()
        {
            base.Setup();
            _buildInformation = new BuildInformation();
        }
    }
}
