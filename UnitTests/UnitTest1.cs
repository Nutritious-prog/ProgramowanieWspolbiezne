
using App;

namespace UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [Test]
        public void Test2()
        {
            Class1 testClass = new Class1();

            Assert.That(testClass.Fib(6), Is.EqualTo(8));
            
        }
    }
}