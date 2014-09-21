using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace SourceIndexingSharp.Tests
{
    [TestFixture]
    public class StringExpanderTests : TestBase
    {
        [Test]
        public void Can_expand_string()
        {
            // arrange
            var value = "This is a test {{VALUE}} with {{PATH}}";
            var variables = new Dictionary<string, string> {{"VALUE", "test..."}};
            var path = Environment.GetEnvironmentVariable("PATH");

            // act
            var result = Context.StringExpander.Expand(value, variables);

            // assert
            Assert.That(result, Is.EqualTo(string.Format("This is a test {0} with {1}", "test...", path)));
        }
    }
}
