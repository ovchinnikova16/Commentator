using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AdditionExample
{
    [TestFixture]
    public class Tests
    {
        public Tests()
        {
        }

        [Test]
        public void TestEqual()
        {
            long a = 10;
            long b = 10;
            Assert.AreEqual(a, b);
        }

        [Test]
        public void TestNotEqual()
        {
            long a = 10;
            long b = 11;
            Assert.AreNotEqual(a, b);
        }
    }
}
