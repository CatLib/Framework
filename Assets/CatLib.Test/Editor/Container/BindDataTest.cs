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
using System.Collections.Generic;
using CatLib.API.Container;
using NUnit.Framework;

namespace CatLib.Test.Container
{
    /// <summary>
    /// 绑定数据测试用例
    /// </summary>
    [TestFixture]
    public class BindDataTest
    {
        #region Needs
        /// <summary>
        /// 需要什么样的数据不为空
        /// </summary>
        [Test]
        public void CheckNeedsIsNotNull()
        {
            var container = new CatLib.Container.Container();
            var bindData = new CatLib.Container.BindData(container, "NeedsIsNotNull", (app, param) => "hello world", false);

            var needs = bindData.Needs("TestService");
            var needsWithType = bindData.Needs<BindDataTest>();
           
            Assert.AreNotEqual(null, needs);
            Assert.AreNotEqual(null, needsWithType);
        }

        /// <summary>
        /// 检测当需求什么方法时传入无效参数
        /// </summary>
        [Test]
        public void CheckNeedsIllegalValue()
        {
            var container = new CatLib.Container.Container();
            var bindData = new CatLib.Container.BindData(container, "CheckNeedsIllegalValue", (app, param) => "hello world", false);

            Assert.Throws<ArgumentNullException>(() =>
            {
                bindData.Needs(null);
                bindData.Needs(string.Empty);
            });
        }

        /// <summary>
        /// 是否可以取得关系上下文
        /// </summary>
        [Test]
        public void CanGetContextual()
        {
            var container = new CatLib.Container.Container();
            var bindData = new CatLib.Container.BindData(container, "NeedsIsNotNull", (app, param) => "hello world", false);

            bindData.Needs("need1").Given("abc");
            bindData.Needs("need2").Given<BindDataTest>();

            Assert.AreEqual("abc", bindData.GetContextual("need1"));
            Assert.AreEqual(typeof(BindDataTest).ToString(), bindData.GetContextual("need2"));
            Assert.AreEqual("empty", bindData.GetContextual("empty"));
        }
        #endregion

        #region Interceptor
        /// <summary>
        /// 添加拦截器 - 拦截器类
        /// </summary>
        private class AddInterceptorClass : IInterception
        {
            /// <summary>
            /// 拦截器是否生效
            /// </summary>
            public bool Enable
            {
                get { return true; }
            }

            /// <summary>
            /// 必须的属性类型才会被拦截
            /// </summary>
            /// <returns>属性列表</returns>
            public IEnumerable<Type> GetRequiredAttr()
            {
                return Type.EmptyTypes;
            }

            /// <summary>
            /// 拦截器方案
            /// </summary>
            /// <param name="methodInvoke">方法调用</param>
            /// <param name="next">下一个拦截器</param>
            /// <returns>拦截返回值</returns>
            public object Interception(IMethodInvoke methodInvoke, Func<object> next)
            {
                return next();
            }
        }

        /// <summary>
        /// 是否能够添加拦截器
        /// </summary>
        [Test]
        public void CanAddInterceptor()
        {
            var container = new CatLib.Container.Container();
            var bindData = new CatLib.Container.BindData(container, "CanAddInterceptor", (app, param) => "hello world", false);

            var newInterceptorClass = new AddInterceptorClass();
            bindData.AddInterceptor<AddInterceptorClass>();
            bindData.AddInterceptor(newInterceptorClass);

            var interceptor = bindData.GetInterceptors();

            Assert.AreNotEqual(null, interceptor);
            Assert.AreEqual(2, interceptor.Length);
            Assert.AreSame(typeof(AddInterceptorClass), interceptor[0].GetType());
            Assert.AreSame(interceptor[1], newInterceptorClass);
            Assert.AreNotSame(interceptor[0], interceptor[1]);
        }

        /// <summary>
        /// 检查是否可以添加非法的拦截器
        /// </summary>
        [Test]
        public void CheckAddIllegalInterceptor()
        {
            var container = new CatLib.Container.Container();
            var bindData = new CatLib.Container.BindData(container, "CheckAddIllegalInterceptor", (app, param) => "hello world", false);

            Assert.Throws<ArgumentNullException>(() =>
            {
                bindData.AddInterceptor(null);
            });
        }
        #endregion

        #region Alias
        /// <summary>
        /// 是否能够增加别名
        /// </summary>
        [Test]
        public void CanAddAlias()
        {
            var container = new CatLib.Container.Container();
            var bindData = container.Bind("CanAddAlias", (app, param) => "hello world", false);

            bindData.Alias("Alias");
            bindData.Alias<BindDataTest>();

            var textAliasGet = container.GetBind("Alias");
            Assert.AreSame(textAliasGet, bindData);

            var classAliasGet = container.GetBind(typeof(BindDataTest).ToString());
            Assert.AreSame(bindData, textAliasGet);
            Assert.AreSame(bindData, classAliasGet);
        }

        /// <summary>
        /// 检测无效的别名
        /// </summary>
        [Test]
        public void CheckIllegalAlias()
        {
            var container = new CatLib.Container.Container();
            var bindData = new CatLib.Container.BindData(container, "CheckIllegalAlias", (app, param) => "hello world", false);

            Assert.Throws<ArgumentNullException>(() =>
            {
                bindData.Alias(null);
                bindData.Alias(string.Empty);
            });
        }
        #endregion

        #region OnResolving
        /// <summary>
        /// 是否能追加到解决事件
        /// </summary>
        [Test]
        public void CanAddOnResolving()
        {
            var container = new CatLib.Container.Container();
            var bindData = new CatLib.Container.BindData(container, "CanAddOnResolving", (app, param) => "hello world", false);

            bindData.OnResolving(obj => null);

            var data = bindData.ExecDecorator(new CatLib.Container.Container());
            Assert.AreEqual(null, data);
        }

        /// <summary>
        /// 检查无效的解决事件传入参数
        /// </summary>
        [Test]
        public void CheckIllegalResolving()
        {
            var container = new CatLib.Container.Container();
            var bindData = new CatLib.Container.BindData(container, "CanAddOnResolving", (app, param) => "hello world", false);

            Assert.Throws<ArgumentNullException>(() =>
            {
                bindData.OnResolving(null);
            });
        }
        #endregion

        #region UnBind
        /// <summary>
        /// 能够正常解除绑定
        /// </summary>
        [Test]
        public void CanUnBind()
        {
            var container = new CatLib.Container.Container();
            var bindData = container.Bind("CanUnBind", (app, param) => "hello world", false);

            Assert.AreEqual("hello world", container.Make("CanUnBind").ToString());
            bindData.UnBind();
            Assert.AreEqual(null, container.Make("CanUnBind"));
        }
        #endregion
    }
}