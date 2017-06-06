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
using System.Collections;
using System.IO;
using CatLib.API;
using CatLib.API.Config;
using CatLib.API.FileSystem;
using CatLib.Config;
using CatLib.Core;
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
    public class FileSystemProviderTests
    {

        public class PrepareEnv : ServiceProvider
        {
            public override IEnumerator Init()
            {
                var path = Path.Combine(Environment.CurrentDirectory, "FileSystemTest");
                App.Make<IEnv>().SetAssetPath(path);
                yield return base.Init();
            }

            public override void Register(){ }
        }

        [TestInitialize]
        public void TestInitialize()
        {
            var path = SIO.Path.Combine(Environment.CurrentDirectory, "FileSystemTest");
            if (SIO.Directory.Exists(path))
            {
                SIO.Directory.Delete(path, true);
            }
            var app = new Application().Bootstrap();
            app.OnFindType((t) =>
            {
                return Type.GetType(t);
            });
            app.Register(typeof(FileSystemProvider));
            app.Register(typeof(CoreProvider));
            app.Register(typeof(PrepareEnv));
            app.Register(typeof(ConfigProvider));
            app.Init();
        }

        [TestMethod]
        public void GetDiskTest()
        {
            TestInitialize();
            var storage = App.Instance.Make<IFileSystemManager>();
            storage.Disk().Write("GetDisk", GetByte("hello world"));
            Assert.AreEqual(true, storage.Disk().Exists("GetDisk"));
            Assert.AreEqual("hello world", GetString(storage.Disk().Read("GetDisk")));
        }

        [TestMethod]
        public void ExtendExistsTest()
        {
            TestInitialize();

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                var storage = App.Instance.Make<IFileSystemManager>();
                storage.Extend(() => new CatLib.FileSystem.FileSystem(new Local(App.Instance.Make<IEnv>().AssetPath)));
            });
        }

        [TestMethod]
        public void DefaultConfigTest()
        {
            TestInitialize();

            var storage = App.Instance.Make<IFileSystemManager>();
            storage.Extend(() => new CatLib.FileSystem.FileSystem(new Local( Path.Combine(App.Instance.Make<IEnv>().AssetPath, "DefaultConfigTest"))) , "local-2");

            var config = App.Instance.Make<IConfigManager>();
            config.Get().Set("filesystems.default" , "local-2");

            storage.Disk().Write("DefaultConfigTest", GetByte("hello world"));
            Assert.AreEqual(true, storage.Disk("local").Exists("DefaultConfigTest/DefaultConfigTest"));
            Assert.AreEqual("hello world" , GetString(storage.Disk("local").Read("DefaultConfigTest/DefaultConfigTest")));
        }

        [TestMethod]
        public void UndefinedResolveTests()
        {
            TestInitialize();
            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                var storage = App.Instance.Make<IFileSystemManager>();
                storage.Disk("undefined-disk");
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
