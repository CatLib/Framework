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

using System;
using System.IO;
using CatLib.API;
using CatLib.FileSystem;
#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
using TestInitialize = NUnit.Framework.SetUpAttribute;
using TestCleanup = NUnit.Framework.TearDownAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Category = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;
#endif

namespace CatLib.Tests.FileSystem
{
    [TestClass]
    public class LocalTests
    {
        /// <summary>
        /// 本地驱动器
        /// </summary>
        private Local local;

        [TestInitialize]
        public void TestInitialize()
        {
            var path = Path.Combine(System.Environment.CurrentDirectory, "FileSystemTest");
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            Directory.CreateDirectory(path);

            local = new Local(path);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            return;
            var path = Path.Combine(System.Environment.CurrentDirectory, "FileSystemTest");
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }

        /// <summary>
        /// 判断一个不存在的文件
        /// </summary>
        [TestMethod]
        public void HasNotExistsFile()
        {
            Assert.AreEqual(false, local.Has("not-existsfile"));
        }

        [TestMethod]
        public void HasExistsTest()
        {
            local.Write("HasWriteToFile.txt", GetByte("hello world"));
            Assert.AreEqual(true, local.Has("HasWriteToFile.txt"));

            //todo create dir
        }

        [TestMethod]
        public void InvalidHasTest()
        {
            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                local.Has("");
            });

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                local.Has(null);
            });

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                local.Has("../hellojumpout.txt");
            });

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                local.Has("../");
            });
        }

        /// <summary>
        /// 写入文件测试
        /// </summary>
        [TestMethod]
        public void WriteToFile()
        {
            local.Write("WriteToFile.txt", GetByte("hello world"));
            local.Write("WriteToFile", GetByte("hello world"));
        }

        /// <summary>
        /// 无效的写入测试
        /// </summary>
        [TestMethod]
        public void InvalidWriteTest()
        {
            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                local.Write("", GetByte("hello world"));
            });
            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                local.Write(null, GetByte("hello world"));
            });

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                local.Write("../hellojumpout.txt", GetByte("hello world"));
            });

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                local.Write("../", GetByte("hello world"));
            });

            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                local.Write("123<>.png", GetByte("hello world"));
            });

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                local.Write("WriteToFile", null);
            });
        }

        private byte[] GetByte(string str)
        {
            return System.Text.Encoding.Default.GetBytes(str);
        }

        private string GetString(byte[] byt)
        {
            return System.Text.Encoding.Default.GetString(byt);
        }
    }
}
