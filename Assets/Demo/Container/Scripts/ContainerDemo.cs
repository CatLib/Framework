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
using UnityEngine;
using CatLib.API.Container;
using System.Collections.Generic;

namespace CatLib.Demo.Container
{

    public class ContainerDemo : MonoBehaviour
    {


        /**
         * Container 是一个特殊的组件，一般情况下CatLib提供的App就继承自Container实例。
         * 但您也可以自建自己的Container.
         */
        public void Start()
        {

            CatLib.Container.Container container = new CatLib.Container.Container();

            container.OnResolving((bind, obj) =>
            {

                Debug.Log("(Global) Container.Resolving() , " + obj.GetType());
                return obj;

            });

            //NormalBindDemo(container);
            AopDemo(container);

        }


        public class NormalBindDemoClass
        {

            public void Call() { Debug.Log("NormalBindDemoClass.Call();"); }

        }

        private void NormalBindDemo(IContainer container)
        {

            container.Bind<NormalBindDemoClass>().OnResolving((obj) =>
            {
                Debug.Log(obj);
                Debug.Log("(Local) Container.Resolving() , " + obj.GetType());
                return obj;

            }).Alias("normal-bind-demo");

            NormalBindDemoClass cls = container.Make("normal-bind-demo") as NormalBindDemoClass;
            //NormalBindDemoClass cls = container.Make<NormalBindDemoClass>();
            //NormalBindDemoClass cls = container["normal-bind-demo"] as NormalBindDemoClass;

            cls.Call();

        }


        [Aop]
        public class AopBindDemoClass : MarshalByRefObject
        {

            //[AOP]
            public void Call()
            {
                Debug.Log("NormalBindDemoClass.Call();");
                aopDemo.Call("abcdefghijklmn");
            }

            [Aop]
            public void Call(string str) { Debug.Log("NormalBindDemoClass.Call(string); " + str); }

        }

        public class Test : System.Attribute { }

        /// <summary>
        /// 拦截器
        /// </summary>
        public class Intercepting : IInterception
        {

            /// <summary>
            /// 是否生效
            /// </summary>
            public bool Enable { get { return true; } }

            /// <summary>
            /// 必须的特性类型（只有被标记这里列出的特性这个拦截器才会生效）
            /// </summary>
            /// <returns></returns>
            public IEnumerable<Type> GetRequiredAttr()
            {
                return Type.EmptyTypes;
            }

            /// <summary>
            /// 拦截器
            /// </summary>
            /// <param name="methodInvoke"></param>
            /// <param name="next"></param>
            /// <returns></returns>
            public object Interception(IMethodInvoke methodInvoke, Func<object> next)
            {
                Debug.Log("befor intercepting");
                var ret = next();
                Debug.Log("after intercepting");
                return ret;
            }
        }

        public static AopBindDemoClass aopDemo;

        private void AopDemo(IContainer container)
        {
            container.Bind<AopBindDemoClass>().OnResolving((obj) =>
            {
                Debug.Log("(Local) Container.Resolving() , " + obj.GetType());
                return obj;

            }).Alias("aop-bind-demo").AddInterceptor<Intercepting>();

            AopBindDemoClass cls = container.Make("aop-bind-demo") as AopBindDemoClass;
            aopDemo = cls;
            cls.Call();
            cls.Call("hello world!");
        }

    }

}