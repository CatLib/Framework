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
    public class DriverTests
    {
        private static string updateResult;
        private static string lateUpdateResult;
        private static bool lateUpdateIsAfter;
        private static string onDestroyResult;

        public class TestStaticClass : IUpdate, ILateUpdate , IDestroy
        {
            public void Update()
            {
                if (updateResult == "TestStaticClassUpdate")
                {
                    updateResult = "TestStaticClassUpdate1";
                }
                else
                {
                    updateResult = "TestStaticClassUpdate";
                }
            }

            public void LateUpdate()
            {
                if (updateResult == "TestStaticClassUpdate")
                {
                    lateUpdateIsAfter = true;
                }
                else
                {
                    lateUpdateIsAfter = false;
                }
                lateUpdateResult = "TestStaticClassLateUpdate";

            }

            public void OnDestroy()
            {
                onDestroyResult = "TestStaticClassDestroy";
            }
        }

        /// <summary>
        /// 静态对象解决时自动载入
        /// </summary>
        [TestMethod]
        public void OnResolvingLoad()
        {
            updateResult = string.Empty;
            var c = MakeDriver();
            c.Singleton<TestStaticClass>();
            c.Make<TestStaticClass>();

            c.Update();
            Assert.AreEqual("TestStaticClassUpdate", updateResult);
        }

        /// <summary>
        /// 释放时自动移除
        /// </summary>
        [TestMethod]
        public void OnReleaseUnLoad()
        {
            updateResult = string.Empty;
            var c = MakeDriver();
            c.Singleton<TestStaticClass>();
            c.Make<TestStaticClass>();

            c.Release<TestStaticClass>();

            c.Update();
            Assert.AreEqual(string.Empty, updateResult);
        }

        /// <summary>
        /// 替换实例的Unload场景
        /// </summary>
        [TestMethod]
        public void ReplaceInstanceToUnload()
        {
            updateResult = string.Empty;
            var c = MakeDriver();
            c.Singleton<TestStaticClass>();
            c.Make<TestStaticClass>();
      
            c.Instance<TestStaticClass>(null);

            c.Update();
            Assert.AreEqual(string.Empty, updateResult);

            ExceptionAssert.DoesNotThrow(() =>
            {
                c.Release<TestStaticClass>();
            });
        }

        /// <summary>
        /// 载入和卸载
        /// </summary>
        [TestMethod]
        public void OnLoadUnLoad()
        {
            updateResult = string.Empty;
            var c = MakeDriver();
            c.Singleton<TestStaticClass>();
            var cls = c.Make<TestStaticClass>();

            c.Update();
            Assert.AreEqual("TestStaticClassUpdate", updateResult);
            c.Detach(cls);
            c.Update();
            Assert.AreEqual("TestStaticClassUpdate", updateResult);
        }

        /// <summary>
        /// 测试驱动和行为
        /// </summary>
        [TestMethod]
        public void TestDriver()
        {
            updateResult = string.Empty;
            var c = MakeDriver();
            c.Singleton<TestStaticClass>();
            var cls = c.Make<TestStaticClass>();

            c.Update();
            c.LateUpdate();
            Assert.AreEqual("TestStaticClassUpdate", updateResult);
            c.Detach(cls);
            c.Update();
            c.LateUpdate();
            Assert.AreEqual("TestStaticClassUpdate", updateResult);
            Assert.AreEqual(true, lateUpdateIsAfter);
            Assert.AreEqual("TestStaticClassDestroy", onDestroyResult);
            Assert.AreEqual("TestStaticClassLateUpdate", lateUpdateResult);
        }

        /// <summary>
        /// 主线程调用
        /// </summary>
        [TestMethod]
        public void MainThreadCall()
        {
            var c = MakeDriver();

            var isCall = false;
            c.MainThread(() =>
            {
                isCall = true;
            });

            Assert.AreEqual(true, isCall);
        }

        private bool isRunCoroutine;

        /// <summary>
        /// 在主线程调用通过协程
        /// </summary>
        [TestMethod]
        public void MainThreadCallWithCoroutine()
        {
            var c = MakeDriver();

            isRunCoroutine = false;
            c.MainThread(Coroutine());

            Assert.AreEqual(true, isRunCoroutine);
        }

        /// <summary>
        /// 开始和停止协同
        /// </summary>
        [TestMethod]
        public void StartAndStopCoroutine()
        {
            var c = MakeDriver();
            isRunCoroutine = false;
            c.StartCoroutine(Coroutine());
            Assert.AreEqual(true, isRunCoroutine);
            c.StopCoroutine(Coroutine());
        }

        /// <summary>
        /// 重复的载入测试
        /// </summary>
        [TestMethod]
        public void RepeatLoadTest()
        {
            var c = MakeDriver();
            var obj = new TestStaticClass();

            c.Attach(obj);

            ExceptionAssert.Throws<RuntimeException>(() =>
            {
                c.Attach(obj);
            });
        }

        /// <summary>
        /// 释放测试
        /// </summary>
        [TestMethod]
        public void OnDestroyTest()
        {
            var c = MakeDriver();
            var obj = new TestStaticClass();

            c.Attach(obj);

            updateResult = string.Empty;
            c.OnDestroy();
            c.Update();

            Assert.AreEqual(string.Empty, updateResult);
        }

        /// <summary>
        /// 卸载没有被加载的对象
        /// </summary>
        [TestMethod]
        public void UnloadNotLoadObj()
        {
            onDestroyResult = string.Empty;
            var c = MakeDriver();
            var obj = new TestStaticClass();
            c.Detach(obj);
            Assert.AreEqual(string.Empty, onDestroyResult);
        }

        /// <summary>
        /// 测试全局On
        /// </summary>
        [TestMethod]
        public void GloablEventOn()
        {
            var app = MakeDriver();
            var isCall = false;
            app.On("GloablEventOn", (sender, e) =>
            {
                isCall = true;
            });

            app.Trigger("GloablEventOn");
            Assert.AreEqual(true, isCall);
        }

        /// <summary>
        /// 测试全局On
        /// </summary>
        [TestMethod]
        public void GloablEventOne()
        {
            var app = MakeDriver();
            var isCall = false;
            app.One("GloablEventOne", (sender, e) =>
            {
                isCall = !isCall;
            });
            app.Trigger("GloablEventOne");
            app.Trigger("GloablEventOne");
            Assert.AreEqual(true, isCall);
        }

        /// <summary>
        /// 根据名称发送者触发事件
        /// </summary>
        [TestMethod]
        public void AppTriggerEventWithSender()
        {
            var app = MakeDriver();
            var isCall = false;
            var sender = new object();
            app.One("AppTriggerEventWithSender", (s, e) =>
            {
                if (s == sender)
                {
                    isCall = !isCall;
                }
            });

            app.Trigger("AppTriggerEventWithSender", sender);
            Assert.AreEqual(true, isCall);
        }

        /// <summary>
        /// 根据名字发送者参数触发事件
        /// </summary>
        [TestMethod]
        public void AppTriggerEventWithSenderArgs()
        {
            var app = MakeDriver();
            var isCall = false;
            var sender = new object();
            var args = new EventArgs();
            app.One("AppTriggerEventWithSenderArgs", (s, e) =>
            {
                if (s == sender && e == args)
                {
                    isCall = !isCall;
                }
            });

            app.Trigger("AppTriggerEventWithSenderArgs", sender, args);
            Assert.AreEqual(true, isCall);
        }

        /// <summary>
        /// 只根据名字和参数触发事件
        /// </summary>
        [TestMethod]
        public void AppTriggerEventWithArgs()
        {
            var app = MakeDriver();
            var isCall = false;
            var args = new EventArgs();
            app.One("AppTriggerEventWithArgs", (s, e) =>
            {
                if (s == null && e == args)
                {
                    isCall = !isCall;
                }
            });

            app.Trigger("AppTriggerEventWithArgs", args);
            Assert.AreEqual(true, isCall);
        }

        private static int destroyNum_DoubleDestroyClass;

        public class DoubleDestroyClass : IDestroy
        {
            /// <summary>
            /// 当释放时
            /// </summary>
            public void OnDestroy()
            {
                destroyNum_DoubleDestroyClass++;
            }
        }
        
        /// <summary>
        /// 防止双重释放
        /// </summary>
        [TestMethod]
        public void TestDoubleDestroyClass()
        {
            destroyNum_DoubleDestroyClass = 0;
            var app = MakeDriver();
            var onRelease = false;
            app.Singleton<DoubleDestroyClass>().OnRelease((bind, obj) =>
            {
                onRelease = true;
            });

            app.Make<DoubleDestroyClass>();
            app.OnDestroy();

            Assert.AreEqual(true, onRelease);
            Assert.AreEqual(1, destroyNum_DoubleDestroyClass);
        }

        private IEnumerator Coroutine()
        {
            yield return null;
            isRunCoroutine = true;
        }

        public Driver MakeDriver()
        {
            var driver = new Application();
            driver.Bootstrap().Init();
            return driver;
        }
    }
}
