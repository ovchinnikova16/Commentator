using System.Reflection.Emit;
using NUnit.Framework;

namespace Commentator.Example.Tests
{
    [TestFixture]
    public class GeneratorTests
    {
        [Test]
        public void Simple()
        {
            var method = new DynamicMethod("Add", typeof(int), new[] { typeof(int), typeof(int) });
            Generator.GenerateAddition(method);
            var result = method.Invoke(null, new object[] { 1, 2 });
            Assert.AreEqual(3, result);
        }
    }
}
