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
using CatLib.Config;
using CatLib.Config.Converters;
using CatLib.Config.Locator;
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
            var config = new CatLib.Config.Config();
            config.AddConverter(typeof(string) , new StringStringConverter());
            config.SetLocator(new CodeConfigLocator());

            Assert.AreEqual(null, config.Get<string>("test"));
            config.Set("test", "test");
            Assert.AreEqual("test", config.Get<string>("test"));

            config.Set("test", "222");
            Assert.AreEqual("222", config.Get<string>("test"));

            config.Save();
        }

        [TestMethod]
        public void NoLocatorTest()
        {
            var config = new CatLib.Config.Config();

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                config.Set("test", "test");
            });
        }

        [TestMethod]
        public void CanNotFindCoverterTest()
        {
            var config = new CatLib.Config.Config();
            config.SetLocator(new CodeConfigLocator());
            ExceptionAssert.Throws<ConverterException>(() =>
            {
                config.Set("123", new ConfigTests());
            });
        }

        [TestMethod]
        public void GetUndefinedTest()
        {
            var config = new CatLib.Config.Config();
            config.SetLocator(new CodeConfigLocator());
            config.Set("123", "123");

            Assert.AreEqual(null, config["222"]);
        }

        [TestMethod]
        public void GetWithUndefinedTypeConverterTest()
        {
            var config = new CatLib.Config.Config();
            config.SetLocator(new CodeConfigLocator());
            config.Set("123", "123");
            Assert.AreEqual(null, config.Get<ConfigTests>("123"));
        }

        [TestMethod]
        public void ExceptionConverterTest()
        {
            var config = new CatLib.Config.Config();
            config.SetLocator(new CodeConfigLocator());
            config.Set("123", "abc");
            
            ExceptionAssert.Throws<ArgumentException>(() =>
            {
                Assert.AreEqual(null, config.Get("123", 0));
            });
        }

        /// <summary>
        /// 保存测试
        /// </summary>
        public void SaveTest()
        {
            var config = new CatLib.Config.Config();
            config.SetLocator(new CodeConfigLocator());
            config.Set("123", "abc");
            config.Save();
        }
    }
}
