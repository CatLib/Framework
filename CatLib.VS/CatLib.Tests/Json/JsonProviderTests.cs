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

using System.Collections.Generic;
using CatLib.API;
using CatLib.API.Json;
using CatLib.Core;
using CatLib.Json;

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

namespace CatLib.Tests.Json
{
    [TestClass]
    public class JsonProviderTests
    {
        /// <summary>
        /// 准备测试环境
        /// </summary>
        /// <returns></returns>
        public IApplication MakeApplication()
        {
            var app = new Application();
            app.Bootstrap();
            app.Register(new JsonProvider());
            app.Init();
            return app;
        }

        public class DemoClass
        {
            public string Name;

            public Dictionary<string, string> Dict;
        }

        [TestMethod]
        public void TestJsonEncodeDecode()
        {
            var app = MakeApplication();
            var json = app.Make<IJson>();
            var demoClass = new DemoClass()
            {
                Name = "helloworld",
                Dict = new Dictionary<string, string>()
                {
                    {"key" , "18" }
                }
            };

            var jsonStr = json.Encode(demoClass);
            var decodeClass = json.Decode<DemoClass>(jsonStr);

            Assert.AreEqual("helloworld", decodeClass.Name);
            Assert.AreEqual("18", decodeClass.Dict["key"]);

            var decodeClassWithObject = json.Decode(jsonStr);

            Assert.AreNotEqual(null, decodeClassWithObject);
        }
    }
}
