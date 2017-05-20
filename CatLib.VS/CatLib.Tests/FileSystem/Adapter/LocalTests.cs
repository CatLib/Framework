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

            local.CreateDir("HasExistsTestDir");
            Assert.AreEqual(true, local.Has("HasExistsTestDir"));
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

        /// <summary>
        /// 读取测试
        /// </summary>
        [TestMethod]
        public void ReadTest()
        {
            local.Write("ReadTest.txt", GetByte("hello world"));
            Assert.AreEqual("hello world", GetString(local.Read("ReadTest.txt")));
        }

        /// <summary>
        /// 无效的读测试
        /// </summary>
        [TestMethod]
        public void InvalidReadTest()
        {
            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                local.Read("");
            });

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                local.Read(null);
            });

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                local.Read("../InvalidReadTest.txt");
            });

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                local.Read("../");
            });

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                local.Read("InvalidReadTest/123123/../../../../123.txt");
            });

            ExceptionAssert.Throws<FileNotFoundException>(() =>
            {
                local.Read("InvalidReadTest.NotExists.txt");
            });
        }

        /// <summary>
        /// 创建文件夹测试
        /// </summary>
        [TestMethod]
        public void CreateDirTest()
        {
            local.CreateDir("CreateDirTest");
            local.CreateDir("CreateDirTest-2/hello/test");
        }

        /// <summary>
        /// 无效的读测试
        /// </summary>
        [TestMethod]
        public void InvalidCreateDirTest()
        {
            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                local.CreateDir("");
            });

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                local.CreateDir(null);
            });

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                local.CreateDir("../test-InvalidCreateDirTest-1");
            });

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                local.CreateDir("123/test/../../../test-InvalidCreateDirTest-2");
            });
        }

        [TestMethod]
        public void RenameDirTest()
        {
            local.CreateDir("RenameTest-norename");
            Assert.AreEqual(true, local.Has("RenameTest-norename"));
            local.Rename("RenameTest-norename", "RenameTest");
            Assert.AreEqual(true, local.Has("RenameTest"));
            Assert.AreEqual(false, local.Has("RenameTest-norename"));
        }

        [TestMethod]
        public void RenameFileTest()
        {
            local.Write("RenameFileTest-norename.txt", GetByte("RenameFileTest"));
            Assert.AreEqual(true, local.Has("RenameFileTest-norename.txt"));
            local.Rename("RenameFileTest-norename.txt", "RenameFileTest");
            Assert.AreEqual(true, local.Has("RenameFileTest"));
            Assert.AreEqual(false, local.Has("RenameFileTest-norename"));
            Assert.AreEqual("RenameFileTest", GetString(local.Read("RenameFileTest")));
        }

        [TestMethod]
        public void InvalidRenameTest()
        {
            local.Write("InvalidRenameTest-norename.txt", GetByte("InvalidRenameTest"));

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                local.Rename("InvalidRenameTest-norename.txt", null);
            });

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                local.Rename("InvalidRenameTest-norename.txt", "");
            });

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                local.Rename("", "InvalidRenameTest");
            });

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                local.Rename(null, "InvalidRenameTest");
            });

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                local.Rename("InvalidRenameTest-norename.txt", "../../test");
            });

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                local.Rename("InvalidRenameTest-norename.txt", "test/../../test");
            });

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                local.Rename("../../InvalidRenameTest-norename.txt", "InvalidRenameTest.txt");
            });

            ExceptionAssert.Throws<IOException>(() =>
            {
                local.Rename("InvalidRenameTest-norename.txt", "hello/InvalidRenameTest.txt");
            });
        }

        [TestMethod]
        public void RenameWithDuplicateNameTest()
        {
            local.Write("RenameWithDuplicateNameTest-norename.txt", GetByte("RenameWithDuplicateNameTest"));
            local.Write("RenameWithDuplicateNameTest.txt", GetByte("hello world"));

            ExceptionAssert.Throws<IOException>(() =>
            {
                local.Rename("InvalidRenameTest-norename.txt", "RenameWithDuplicateNameTest.txt");
            });

            Assert.AreEqual("hello world", GetString(local.Read("RenameWithDuplicateNameTest.txt")));
        }

        [TestMethod]
        public void RenameDirWithDuplicateNameTest()
        {
            local.CreateDir("RenameDirWithDuplicateNameTest");
            local.Write("RenameDirWithDuplicateNameTest/RenameWithDuplicateNameTest-norename.txt", GetByte("hello"));
            local.Write("RenameDirWithDuplicateNameTest/RenameDirWithDuplicateNameTest.txt", GetByte("world"));
            local.Write("RenameDir", GetByte("111"));

            ExceptionAssert.Throws<IOException>(() =>
            {
                local.Rename("RenameDirWithDuplicateNameTest", "RenameDir");
            });

            Assert.AreEqual("111", GetString(local.Read("RenameDir")));
        }

        [TestMethod]
        public void CopyDirTest()
        {
            local.CreateDir("CopyDir-norename/Test1");
            local.CreateDir("CopyDir-norename/Test2");

            local.Write("CopyDir-norename/Test1/text11.txt", GetByte("test11"));
            local.Write("CopyDir-norename/Test1/text12.txt", GetByte("test12"));
            local.Write("CopyDir-norename/Test2/text21.txt", GetByte("test21"));
            local.Write("CopyDir-norename/maintxt.txt", GetByte("test21"));

            local.Copy("CopyDir-norename", "TestCopy/CopyDir");
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
