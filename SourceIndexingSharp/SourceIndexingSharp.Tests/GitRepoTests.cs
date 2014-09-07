using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SourceIndexingSharp.Git;

namespace SourceIndexingSharp.Tests
{
    [TestFixture]
    public class GitRepoTests
    {
        [Test]
        public void Can_get_repository_information()
        {
            // arrange /act
            using (var repo = new GitRepo(new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.Parent.FullName))
            {
                // assert
                Assert.That(repo.Sha, Is.Not.Null.Or.Empty);
                Assert.That(repo.Checksums, Has.Count.GreaterThan(0));
            }
        }
    }
}
