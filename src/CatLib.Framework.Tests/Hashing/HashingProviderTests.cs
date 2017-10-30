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
using CatLib.Hashing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CatLib.Tests.Hashing
{
    [TestClass]
    public class HashingProviderTests
    {
        public IApplication MakeEnv()
        {
            var app = new Application();
            app.Bootstrap();
            app.Register(new HashingProvider());
            app.Init();
            return app;
        }

        [TestMethod]
        public void TestCrc64()
        {
            var app = MakeEnv();
            var hash = app.Make<IHashing>();
            var code0 = hash.Checksum(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            var code = hash.Checksum(new byte[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10}, Checksums.Crc32);
            var code2 = hash.Checksum(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, Checksums.Crc32);
            var code3 = hash.Checksum(new byte[] { 0, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, Checksums.Crc32);
            var code4 = hash.Checksum(System.Text.Encoding.Default.GetBytes("123"), Checksums.Crc32);
            var code5 = hash.Checksum(System.Text.Encoding.Default.GetBytes("123"));

            Assert.AreEqual(code, code0);
            Assert.AreEqual(code , code2);
            Assert.AreNotEqual(code2 , code3);
            Assert.AreEqual(2286445522, code4);
            Assert.AreEqual(2286445522, code5);
        }

        [TestMethod]
        public void TestMultHash()
        {
            var app = MakeEnv();
            var hash = app.Make<IHashing>();

            var data = new byte[][] {
                new byte[]{ 1,2,3 },
                new byte[]{ 4,5,6,7 },
                new byte[]{ 8,9,10 },
            };

            var code0 = hash.Checksum(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            var code1 = hash.Checksum((checksum) =>
            {
                checksum(data[0], 0, data[0].Length);
                checksum(data[1], 0, data[1].Length);
                checksum(data[2], 0, data[2].Length);
            }, Checksums.Crc32);

            var code2 = hash.Checksum((checksum) =>
            {
                checksum(data[0], 0, data[0].Length);
                checksum(data[1], 2, data[1].Length - 2);
                checksum(data[2], 0, data[2].Length);
            }, Checksums.Crc32);

            Assert.AreEqual(code0, code1);
            Assert.AreNotEqual(code1, code2);
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

        [TestMethod]
        public void TestUndefiendChecksums()
        {
            var app = MakeEnv();
            var hash = app.Make<IHashing>();

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                hash.Checksum(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, (Checksums.Crc32 + 9999));
            });
        }

        [TestMethod]
        public void TestHashPassword()
        {
            var app = MakeEnv();
            var hash = app.Make<IHashing>();

            var pass = hash.HashPassword("helloworld", 10);
            Assert.AreEqual(true, hash.CheckPassword("helloworld", pass));

            var pass2 = hash.HashPassword("helloworld",8);
            Assert.AreEqual(true, hash.CheckPassword("helloworld", pass2));

            var pass3 = hash.HashPassword(string.Empty, 8);
            Assert.AreEqual(true, hash.CheckPassword(string.Empty, pass3));
        }

        [TestMethod]
        public void TestMurmurHash()
        {
            var app = MakeEnv();
            var hash = app.Make<IHashing>();

            var hash0 = hash.Checksum("helloworld",Checksums.Murmur32);
            var hash1 = hash.Checksum("helloworld", Checksums.Murmur32);
            var hash2 = hash.Checksum("helloworld", Checksums.Murmur32);
            var hash3 = hash.Checksum("helloworl", Checksums.Murmur32);
            var hash4 = hash.Checksum(System.Text.Encoding.Default.GetBytes("helloworl"), Checksums.Murmur32);

            Assert.AreEqual(hash1, hash0);
            Assert.AreEqual(hash1, hash2);
            Assert.AreNotEqual(hash2, hash3);
            Assert.AreEqual(hash3, hash4);
        }

        [TestMethod]
        public void TestDjbHash()
        {
            var app = MakeEnv();
            var hash = app.Make<IHashing>();

            var hash1 = hash.Checksum("helloworld", Checksums.Djb);
            var hash2 = hash.Checksum("helloworld", Checksums.Djb);
            var hash3 = hash.Checksum("helloworl", Checksums.Djb);

            Assert.AreEqual(hash1, hash2);
            Assert.AreNotEqual(hash2, hash3);
        }
    }
}
