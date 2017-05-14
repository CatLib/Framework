/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Category = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;
#endif

namespace CatLib.Tests.FilterChain
{
    [TestClass]
    public class FilterChainTests
    {
        [TestMethod]
        public void TestFilterChainT1()
        {
            var filter = new CatLib.FilterChain.FilterChain();

            var chain = filter.Create<string>();

            var data = "123";
            var isCall = false;
            chain.Add((inData, next) =>
            {
                if (inData == data)
                {
                    isCall = true;
                }
                next(inData);
            });
            chain.Add((in1Data, next) =>
            {
                next(in1Data);
            });
            var completeCall = false;
            chain.Do(data, (in1) =>
            {
                completeCall = true;
            });

            Assert.AreEqual(2, chain.FilterList.Length);
            Assert.AreEqual(true, isCall);
            Assert.AreEqual(true, completeCall);
        }

        [TestMethod]
        public void NoFilterChainCallT1()
        {
            var filter = new CatLib.FilterChain.FilterChain();
            var chain = filter.Create<string>();

            var isCall = false;
            chain.Do("", (i) =>
            {
                isCall = !isCall;
            });

            chain.Do("");
            Assert.AreEqual(true, isCall);
        }

        [TestMethod]
        public void RecursiveCallChain()
        {
            var filter = new CatLib.FilterChain.FilterChain();

            var chain = filter.Create<string>();

            var data = "123";
            var isCall = false;
            chain.Add((inData, next) =>
            {
                if (inData == data)
                {
                    isCall = !isCall;
                }
                if (isCall)
                {
                    chain.Do(data, (in1) =>
                    {
                        
                    });
                }
                next(inData);
            });

            chain.Add((in1Data, next) =>
            {
                next(in1Data);
            });

            var completeCall = false;
            chain.Do(data, (in1) =>
            {
                completeCall = true;
            });
            Assert.AreEqual(false, isCall);
            Assert.AreEqual(true, completeCall);
        }

        [TestMethod]
        public void TestFilterChainT1T2()
        {
            var filter = new CatLib.FilterChain.FilterChain();

            var chain = filter.Create<string,string>();

            var data1 = "123";
            var data2 = "222";

            var isCall = false;
            chain.Add((in1Data,in2Data, next) =>
            {
                if (in1Data == data1 && in2Data == data2)
                {
                    isCall = true;
                }
                next(in1Data, in2Data);
            });

            chain.Add((in1Data, in2Data, next) =>
            {
                next(in1Data, in2Data);
            });

            var completeCall = false;
            chain.Do(data1, data2 , (in1, in2) =>
            {
                completeCall = true;
            });
            Assert.AreEqual(2, chain.FilterList.Length);
            Assert.AreEqual(true, isCall);
            Assert.AreEqual(true, completeCall);
        }

        [TestMethod]
        public void NoFilterChainCallT1T2()
        {
            var filter = new CatLib.FilterChain.FilterChain();
            var chain = filter.Create<string,string>();

            var isCall = false;
            chain.Do("","", (i,n) =>
            {
                isCall = !isCall;
            });

            chain.Do("","");
            Assert.AreEqual(true, isCall);
        }

        [TestMethod]
        public void TestFilterChainT1T2T3()
        {
            var filter = new CatLib.FilterChain.FilterChain();

            var chain = filter.Create<string, string, string>();

            var data1 = "123";
            var data2 = "222";
            var data3 = "333";

            var isCall = false;
            chain.Add((in1Data, in2Data , in3Data, next) =>
            {
                if (in1Data == data1 && in2Data == data2 && in3Data == data3)
                {
                    isCall = true;
                }
                next(in1Data, in2Data , in3Data);
            });

            chain.Add((in1Data, in2Data, in3Data, next) =>
            {
                next(in1Data, in2Data, in3Data);
            });

            var completeCall = false;
            chain.Do(data1, data2, data3, (in1, in2, in3) =>
            {
                completeCall = true;
            });

            Assert.AreEqual(2, chain.FilterList.Length);
            Assert.AreEqual(true, isCall);
            Assert.AreEqual(true, completeCall);
        }

        [TestMethod]
        public void NoFilterChainCallT1T2T3()
        {
            var filter = new CatLib.FilterChain.FilterChain();
            var chain = filter.Create<string, string,string>();

            var isCall = false;
            chain.Do("", "", "", (i, n , f) =>
            {
                isCall = !isCall;
            });

            chain.Do("", "" , "");
            Assert.AreEqual(true, isCall);
        }
    }
}
