using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperMath;

namespace TestMath
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestSimpleAdd()
        {
            const int lh = 100;
            const int rh = 1;
            const int result = lh + rh;

            var testlh = Decimal3.Parse(lh);
            var testrh = Decimal3.Parse(rh);
            var test = testlh + testrh;

            Assert.AreEqual(result, test.Value);
        }

        [TestMethod]
        public void TestCarryAdd()
        {
            var lh = 1;
            var rh = 2;
            var result = lh + rh;

            var testlh = Decimal3.Parse(lh);
            var testrh = Decimal3.Parse(rh);
            var test = testlh + testrh;

            Assert.AreEqual(result, test.Value);
        }

        [TestMethod]
        public void TestSub()
        {
            var lh = 10;
            var rh = 1;
            var result = lh - rh;

            var testlh = Decimal3.Parse(lh);
            var testrh = Decimal3.Parse(rh);
            var test = testlh - testrh;

            Assert.AreEqual(result, test.Value);
        }

        [TestMethod]
        public void TestCarrySub()
        {
            var lh = 10;
            var rh = 12;
            var result = lh - rh;

            var tl = Decimal3.Parse(lh);
            var tr = Decimal3.Parse(rh);
            var test = tl - tr;

            Assert.AreEqual(result, test.Value);
        }

        [TestMethod]
        public void TestGoPositive()
        {
            var lh = -1;
            var rh = 5;
            var result = lh + rh;

            var tl = Decimal3.Parse(lh);
            var tr = Decimal3.Parse(rh);
            var test = tl + tr;

            Assert.AreEqual(result, test.Value);
        }

        [TestMethod]
        public void TestDoubleNegative()
        {
            var lh = -1;
            var rh = -2;
            var result = lh + rh;

            var testlh = Decimal3.Parse(lh);
            var testrh = Decimal3.Parse(rh);
            var test = testlh + testrh;

            Assert.AreEqual(result, test.Value);
        }

        [TestMethod]
        public void TestMultInverse()
        {
            var lh = 5;
            var rh = -1;
            var result = lh * rh;

            var l = Decimal3.Parse(lh);
            var r = Decimal3.Parse(rh);
            var test = l * r;

            Assert.AreEqual(result, test.Value);

            test = r * l;
            Assert.AreEqual(result, test.Value);
        }

        [TestMethod]
        public void TestMult()
        {
            var lh = 5;
            var rh = 5;
            var result = lh * rh;

            var l = Decimal3.Parse(lh);
            var r = Decimal3.Parse(rh);
            var test = l * r;

            Assert.AreEqual(result, test.Value);
        }

        [TestMethod]
        public void TestDiv()
        {
            var lh = 3;
            var rh = 2;
            var result = lh / rh;

            var r = Decimal3.Parse(rh);
            var l = Decimal3.Parse(lh);
            var test = l / r;

            Assert.AreEqual(result, test.Value);
        }

        [TestMethod]
        public void TestWtf()
        {
            const int two = 2;

            const int result = two + two - two;
            var twoD = Decimal3.Parse(two);
            var test = twoD + twoD - twoD;

            Assert.AreEqual(result, test.Value);
        }

        [TestMethod]
        public void TestStress()
        {
            var big1 = 123456789;
            var big2 = 12345;
            var result = big1 + big2 * big2 / big1;

            var bigD2 = Decimal3.Parse(big2);
            var bigD1 = Decimal3.Parse(big1);
            var test = bigD1 + bigD2 * bigD2 / bigD1;

            Assert.AreEqual(result, test.Value);
        }
    }
}
