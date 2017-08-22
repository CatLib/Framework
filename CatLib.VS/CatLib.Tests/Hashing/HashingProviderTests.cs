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

using CatLib.API.Hashing;
using CatLib.Events;
using CatLib.Hashing;

#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

namespace CatLib.Tests.Hashing
{
    [TestClass]
    public class HashingProviderTests
    {
        public IApplication MakeEnv()
        {
            var app = new Application();
            app.Bootstrap();
            app.Register(new EventsProvider());
            app.Register(new HashingProvider());
            app.Init();
            return app;
        }

        [TestMethod]
        public void TestCrc64()
        {
            var app = MakeEnv();
            var hash = app.Make<IHashing>();
            var code = hash.Checksum(new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10}, Checksums.Crc32);
            var code2 = hash.Checksum(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, Checksums.Crc32);
            var code3 = hash.Checksum(new byte[] { 0, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, Checksums.Crc32);
            var code4 = hash.Checksum(System.Text.Encoding.Default.GetBytes("123"), Checksums.Crc32);

            Assert.AreEqual(code , code2);
            Assert.AreNotEqual(code2 , code3);
            Assert.AreEqual(2286445522, code4);
        }

        [TestMethod]
        public void TestAdler32()
        {
            var app = MakeEnv();
            var hash = app.Make<IHashing>();
            var code = hash.Checksum(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, Checksums.Adler32);
            var code2 = hash.Checksum(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, Checksums.Adler32);
            var code3 = hash.Checksum(new byte[] { 0, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, Checksums.Adler32);
            var code4 = hash.Checksum(System.Text.Encoding.Default.GetBytes("123"), Checksums.Adler32);

            Assert.AreEqual(code, code2);
            Assert.AreNotEqual(code2, code3);
            Assert.AreEqual(19726487, code4);
        }
    }
}
