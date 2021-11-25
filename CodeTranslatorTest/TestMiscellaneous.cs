using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using CodeTranslator.Utility;

namespace CodeTranslatorTest
{
    internal class TestMiscellaneous
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestSwappingTwoNumbers()
        {
            int a = 10, b = 20;
            Console.WriteLine("{0}, {1}", a, b);
            (a, b) = (b, a);
            Console.WriteLine("{0}, {1}", a, b);
            Assert.AreEqual(20, a);
            Assert.AreEqual(10, b);
        }

        private int _a = 30, _b = 10;
        [Test]
        public void TestSwappingTwoNumbers_1()
        {
            Console.WriteLine("{0}, {1}", _a, _b);
            (_a, _b) = (_b, _a);
            Console.WriteLine("{0}, {1}", _a, _b);
            Assert.AreEqual(30, _b);
            Assert.AreEqual(10, _a);
        }
    }
}
