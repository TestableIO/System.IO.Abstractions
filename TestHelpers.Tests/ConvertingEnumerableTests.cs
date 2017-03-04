using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO.Abstractions;
namespace System.IO.Abstractions.TestingHelpers.Tests
{
    [TestFixture]
    public class ConvertingEnumerableTests
    {
        [Test]
        public void Ctor_Arg1_Null_Throws_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var instance = new ConvertingEnumerable<string, string>(null, (x)=>x);
            });
        }

        [Test]
        public void Ctor_Arg2_Null_Throws_ArgumentNullException()
        {
            var stringarray = new string[] { "1", "2", "3" };

            Assert.Throws<ArgumentNullException>(() =>
            {
                var instance = new ConvertingEnumerable<string, string>(stringarray, null);
            });
        }

    }
}
