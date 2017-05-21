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
using CatLib.API.FileSystem;
using CatLib.FileSystem;
using SIO = System.IO;
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
            var path = Path.Combine(Environment.CurrentDirectory, "FileSystemTest");
            if (SIO.Directory.Exists(path))
            {
                SIO.Directory.Delete(path, true);
            }
            SIO.Directory.CreateDirectory(path);

            local = new Local(path);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "FileSystemTest");
            if (SIO.Directory.Exists(path))
            {
                SIO.Directory.Delete(path, true);
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
        public void RenameDuplicateDir()
        {
            local.CreateDir("RenameDuplicateDir-norename");
            local.CreateDir("RenameDuplicateDir");

            ExceptionAssert.Throws<IOException>(() =>
            {
                local.Rename("RenameDuplicateDir-norename", "RenameDuplicateDir");
            });
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
            local.CreateDir("CopyDir-norename/Test2/SubDir");

            local.Write("CopyDir-norename/Test1/text11.txt", GetByte("test11"));
            local.Write("CopyDir-norename/Test1/text12.txt", GetByte("test12"));
            local.Write("CopyDir-norename/Test2/text21.txt", GetByte("test21"));
            local.Write("CopyDir-norename/Test2/SubDir/text21-sub.txt", GetByte("test21-sub"));
            local.Write("CopyDir-norename/maintxt.txt", GetByte("test21"));

            local.Copy("CopyDir-norename", "TestCopy/CopyDir");

            Assert.AreEqual("test11", GetString(local.Read("CopyDir-norename/Test1/text11.txt")));
            Assert.AreEqual("test12", GetString(local.Read("CopyDir-norename/Test1/text12.txt")));
            Assert.AreEqual("test21", GetString(local.Read("CopyDir-norename/Test2/text21.txt")));
            Assert.AreEqual("test21-sub", GetString(local.Read("CopyDir-norename/Test2/SubDir/text21-sub.txt")));
            Assert.AreEqual("test21", GetString(local.Read("CopyDir-norename/maintxt.txt")));

            Assert.AreEqual("test11", GetString(local.Read("TestCopy/CopyDir/Test1/text11.txt")));
            Assert.AreEqual("test12", GetString(local.Read("TestCopy/CopyDir/Test1/text12.txt")));
            Assert.AreEqual("test21", GetString(local.Read("TestCopy/CopyDir/Test2/text21.txt")));
            Assert.AreEqual("test21-sub", GetString(local.Read("TestCopy/CopyDir/Test2/SubDir/text21-sub.txt")));
            Assert.AreEqual("test21", GetString(local.Read("TestCopy/CopyDir/maintxt.txt")));
        }

        [TestMethod]
        public void CopyFileTest()
        {
            local.Write("CopyFile-norename/text11.txt", GetByte("hello world"));
            local.Copy("CopyFile-norename/text11.txt", "TestFileCopy");

            Assert.AreEqual("hello world", GetString(local.Read("CopyFile-norename/text11.txt")));
            Assert.AreEqual("hello world", GetString(local.Read("TestFileCopy/text11.txt")));
        }

        [TestMethod]
        public void InvalidCopyTest()
        {
            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                local.Copy("", "123.txt");
            });

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                local.Copy(null, "123.txt");
            });

            ExceptionAssert.Throws<FileNotFoundException>(() =>
            {
                local.Copy("not-exists-copy-dir", "123.txt");
            });

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                local.Copy("test", null);
            });

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                local.Copy("test", "");
            });

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                local.Copy("../../test", "test.txt");
            });

            local.Write("InvalidCopyTest.txt", GetByte("123"));
            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                local.Copy("InvalidCopyTest.txt", "../test.txt");
            });
        }

        [TestMethod]
        public void DeleteFileTest()
        {
            Assert.AreEqual(false, local.Has("DeleteTest/test.txt"));
            local.Write("DeleteTest/test.txt", GetByte("test"));
            Assert.AreEqual(true, local.Has("DeleteTest/test.txt"));
            local.Delete("DeleteTest/test.txt");
            Assert.AreEqual(false, local.Has("DeleteTest/test.txt"));
        }

        [TestMethod]
        public void DeleteDirTest()
        {
            Assert.AreEqual(false, local.Has("DeleteDirTest"));
            local.CreateDir("DeleteDirTest");
            local.Write("DeleteDirTest/test.txt", GetByte("test"));
            Assert.AreEqual(true, local.Has("DeleteDirTest"));
            Assert.AreEqual(true, local.Has("DeleteDirTest/test.txt"));
            Assert.AreEqual("test", GetString(local.Read("DeleteDirTest/test.txt")));
            local.Delete("DeleteDirTest");
            Assert.AreEqual(false, local.Has("DeleteDirTest"));
        }

        [TestMethod]
        public void InvalidDeleteTest()
        {
            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                local.Delete("");
            });

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                local.Delete(null);
            });

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                local.Delete("../");
            });
        }

        [TestMethod]
        public void GetAttributesTest()
        {
            local.CreateDir("GetAttributesTest");
            local.Write("GetAttributesTest/test.txt", GetByte("test"));

            Assert.AreEqual(FileAttributes.Directory, local.GetAttributes("GetAttributesTest") & FileAttributes.Directory);
            Assert.AreEqual((FileAttributes)0, local.GetAttributes("GetAttributesTest/test.txt") & FileAttributes.Directory);
        }

        [TestMethod]
        public void InvalidGetAttributesTest()
        {
            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                local.GetAttributes("");
            });

            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                local.GetAttributes(null);
            });
            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                local.GetAttributes("../../../");
            });
        }

        [TestMethod]
        public void GetInfoTest()
        {
            local.CreateDir("GetInfoTest");
            local.Write("GetInfoTest/test.txt", GetByte("test"));

            Assert.AreNotEqual(null, local.GetInfo("GetInfoTest"));
            Assert.AreNotEqual(null, local.GetInfo("GetInfoTest/test.txt"));
        }

        [TestMethod]
        public void InvalidGetInfoTest()
        {
            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                local.GetInfo("");
            });
            ExceptionAssert.Throws<ArgumentNullException>(() =>
            {
                local.GetInfo(null);
            });
            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                local.GetInfo("../../../");
            });
            ExceptionAssert.Throws<FileNotFoundException>(() =>
            {
                local.GetInfo("InvalidGetInfoTest.txt");
            });
        }

        [TestMethod]
        public void GetListTest()
        {
            local.CreateDir("GetListTest");
            local.Write("GetListTest/test.txt", GetByte("test"));

            var lst = local.GetList("GetListTest");
            Assert.AreEqual(1, lst.Length);
            Assert.AreEqual("\\FileSystemTest\\GetListTest\\test.txt", lst[0].Substring(Environment.CurrentDirectory.Length));

            Assert.AreEqual(true, local.GetList("").Length > 0);
            Assert.AreEqual(true, local.GetList(null).Length > 0);
        }

        [TestMethod]
        public void GetListWithFile()
        {
            local.CreateDir("GetListWithFile");
            local.Write("GetListWithFile/GetListWithFile.txt", GetByte("test"));
            var lst = local.GetList("GetListWithFile/GetListWithFile.txt");
            Assert.AreEqual(1, lst.Length);
            Assert.AreEqual("\\FileSystemTest\\GetListWithFile\\GetListWithFile.txt", lst[0].Substring(Environment.CurrentDirectory.Length));
        }

        [TestMethod]
        public void InvalidGetListTest()
        {
            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                local.GetList("../../../");
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
