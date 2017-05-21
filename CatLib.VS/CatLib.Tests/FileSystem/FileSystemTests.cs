using System;
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
    public class FileSystemTests
    {
        private CatLib.FileSystem.FileSystem fileSystem;

        [TestMethod]
        public void FileSystemCreateDirTest()
        {
            Env(() =>
            {
                Assert.AreEqual(false, fileSystem.Exists("FileSystemCreateDirTest-dir"));
                fileSystem.CreateDir("FileSystemCreateDirTest-dir");
                Assert.AreEqual(true, fileSystem.Exists("FileSystemCreateDirTest-dir"));

                Assert.AreEqual(false, fileSystem.Exists("FileSystemCreateDirTest2-dir/hello/world"));
                fileSystem.CreateDir("FileSystemCreateDirTest2-dir/hello/world");
                Assert.AreEqual(true, fileSystem.Exists("FileSystemCreateDirTest2-dir/hello/world"));
            });
        }

        private void Env(Action action)
        {
            var path = SIO.Path.Combine(Environment.CurrentDirectory, "FileSystemTest");
            if (SIO.Directory.Exists(path))
            {
                SIO.Directory.Delete(path, true);
            }
            SIO.Directory.CreateDirectory(path);

            var local = new Local(path);
            fileSystem = new CatLib.FileSystem.FileSystem(local);

            action.Invoke();

            if (SIO.Directory.Exists(path))
            {
                SIO.Directory.Delete(path, true);
            }
        }
    }
}
