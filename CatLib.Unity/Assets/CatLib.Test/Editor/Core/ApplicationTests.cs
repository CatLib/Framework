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
using CatLib.API;
using CatLib.Core;
using CatLib.Event;
#if UNITY_EDITOR || NUNIT
using NUnit.Framework;
using TestClass = NUnit.Framework.TestFixtureAttribute;
using TestMethod = NUnit.Framework.TestAttribute;
#else
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Category = Microsoft.VisualStudio.TestTools.UnitTesting.DescriptionAttribute;
#endif


namespace CatLib.Tests.Core
{
    [TestClass]
    public class ApplicationTests
    {
        [TestMethod]
        public void RepeatInitTest()
        {
            var app = MakeApplication();
            app.Init();
        }

        /// <summary>
        /// 未经引导的初始化
        /// </summary>
        [TestMethod]
        public void NoBootstrapInit()
        {
            var app = new Application();

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                app.Init();
            });
        }

        /// <summary>
        /// 获取版本号测试
        /// </summary>
        [TestMethod]
        public void GetVersionTest()
        {
            var app = MakeApplication();
            Assert.AreNotEqual(string.Empty, app.Version);
        }

        /// <summary>
        /// 获取当前启动流程
        /// </summary>
        [TestMethod]
        public void GetCurrentProcess()
        {
            var app = MakeApplication();
            Assert.AreEqual(Application.StartProcess.OnComplete, app.Process);
        }

        /// <summary>
        /// 重复的引导测试
        /// </summary>
        [TestMethod]
        public void RepeatBootstrap()
        {
            var app = new Application();
            app.Bootstrap();
            app.Init();
            app.Bootstrap();
            Assert.AreEqual(Application.StartProcess.OnComplete, app.Process);
        }

        /// <summary>
        /// 注册非法类型测试
        /// </summary>
        [TestMethod]
        public void RegisteredIllegalType()
        {
            var app = new Application();
            app.Bootstrap();
            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                app.Register(typeof(ApplicationTests));
            });
        }

        /// <summary>
        /// 重复的注册
        /// </summary>
        [TestMethod]
        public void RepeatRegister()
        {
            var app = MakeApplication();

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                app.Register(typeof(EventProvider));
            });
        }

        /// <summary>
        /// 获取唯一标识符
        /// </summary>
        [TestMethod]
        public void GetGuid()
        {
            var app = MakeApplication();
            Assert.AreNotEqual(app.GetGuid(), app.GetGuid());
        }

        private static bool prioritiesTest;

        private static bool inited;

        private class ProviderTest1 : ServiceProvider
        {
            [CatLib.API.Priority(10)]
            public override IEnumerator OnProviderProcess()
            {
                prioritiesTest = true;
                yield break;
            }

            public override void Register()
            {

            }
        }

        [CatLib.API.Priority(5)]
        private class ProviderTest2 : ServiceProvider
        {
            public override void Init()
            {
                inited = true;
                base.Init();
            }

            public override IEnumerator OnProviderProcess()
            {
                prioritiesTest = false;
                yield break;
            }

            public override void Register()
            {

            }
        }

        /// <summary>
        /// 优先级测试
        /// </summary>
        [TestMethod]
        public void ProvidersPrioritiesTest()
        {
            var app = new Application();
            app.OnFindType((t) =>
            {
                return Type.GetType(t);
            });
            app.Bootstrap();
            App.Instance.Register(typeof(ProviderTest1));
            App.Instance.Register(typeof(ProviderTest2));
            app.Init();
            Assert.AreEqual(true, prioritiesTest);
        }

        /// <summary>
        /// 无效的引导
        /// </summary>
        [TestMethod]
        public void IllegalBootstrap()
        {
            var app = new Application();
            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                app.Bootstrap(typeof(ApplicationTests)).Init();
            });
        }

        /// <summary>
        /// 初始化后再注册
        /// </summary>
        [TestMethod]
        public void InitedAfterRegister()
        {
            inited = false;
            prioritiesTest = true;
            var app = new Application();
            app.OnFindType((t) =>
            {
                return Type.GetType(t);
            });
            app.Bootstrap();
            App.Instance.Register(typeof(ProviderTest1));
            app.Init();
            App.Instance.Register(typeof(ProviderTest2));
            Assert.AreEqual(true , inited);
            Assert.AreEqual(false , prioritiesTest);
        }

        private Application MakeApplication()
        {
            var app = new Application();
            app.OnFindType((t) =>
            {
                return Type.GetType(t);
            });
            app.Bootstrap(typeof(BootstrapClass)).Init();
            return app;
        }

        private class BootstrapClass : IBootstrap
        {
            public void Bootstrap()
            {
                App.Instance.Register(typeof(EventProvider));
            }
        }
    }
}
