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
using CatLib.API;
using CatLib.API.Config;
using CatLib.API.Converters;
using CatLib.Config;
using CatLib.Config.Locator;
using CatLib.Converters;
using CatLib.Converters.Plan;
using CatLib.Core;
#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Category = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;
#endif

namespace CatLib.Tests.Config
{
    [TestClass]
    public class ConfigTests
    {
        [TestMethod]
        public void ConfigTest()
        {
            var converent = new CatLib.Converters.Converters();
            converent.AddConverter(new StringStringConverter());
            var config = new CatLib.Config.Config(null, null);
            config.SetConverters(converent);
            config.SetLocator(new CodeConfigLocator());

            Assert.AreEqual(null, config.Get<string>("test"));
            config.Set("test", "test");
            Assert.AreEqual("test", config.Get<string>("test"));

            config.Set("test", "222");
            Assert.AreEqual("222", config.Get<string>("test"));

            config.Save();
        }

        [TestMethod]
        public void TestDefaultIConfig()
        {
            var app = new Application();
            app.Bootstrap();
            app.Register(new ConfigProvider());
            app.Register(new ConvertersProvider());
            app.Init();

            Assert.AreSame(app.Make<IConfigManager>().Default, app.Make<IConfig>());
        }

        [TestMethod]
        public void NoLocatorTest()
        {
            var config = new CatLib.Config.Config(new CatLib.Converters.Converters(), null);

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                config.Set("test", "test");
            });
        }

        [TestMethod]
        public void TestNoConverts()
        {
            var config = new CatLib.Config.Config(null, new CodeConfigLocator());
            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                config.Set("test", "test");
            });
        }

        [TestMethod]
        public void GetUndefinedTest()
        {
            var converent = new CatLib.Converters.Converters();
            converent.AddConverter(new StringStringConverter());
            var config = new CatLib.Config.Config(converent, new CodeConfigLocator());
            config.SetLocator(new CodeConfigLocator());
            config.Set("123", "123");

            Assert.AreEqual(null, config["222"]);
        }

        [TestMethod]
        public void GetWithUndefinedTypeConverterTest()
        {
            var converent = new CatLib.Converters.Converters();
            converent.AddConverter(new StringStringConverter());
            var config = new CatLib.Config.Config(converent, new CodeConfigLocator());
            config.SetLocator(new CodeConfigLocator());
            config.Set("123", "123");

            Assert.AreEqual(null, config.Get<ConfigTests>("123"));
        }

        [TestMethod]
        public void ExceptionConverterTest()
        {
            var converent = new CatLib.Converters.Converters();
            converent.AddConverter(new StringStringConverter());
            converent.AddConverter(new StringInt32Converter());
            var config = new CatLib.Config.Config(converent , new CodeConfigLocator());
            config.Set("123", "abc");
            Assert.AreEqual(0, config.Get("123", 0));
        }

        /// <summary>
        /// 保存测试
        /// </summary>
        public void SaveTest()
        {
            var converent = new CatLib.Converters.Converters();
            converent.AddConverter(new StringStringConverter());
            var config = new CatLib.Config.Config(converent , new CodeConfigLocator());
            config.Set("123", "abc");
            config.Save();
        }
    }
}
