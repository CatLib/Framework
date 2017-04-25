/*
 * This file is part of the CatLib package.
 *
 * (c) Ming ming <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using System;
using CatLib.API;
using CatLib.Event;
using CatLib.Lua;

namespace CatLib.Demo.Lua
{
    /**
     * 这个类提供了当前demo演示时用到的组件 
     */
    public class Bootstraps : IBootstrap
    {

        public void Bootstrap()
        {
            App.Instance.Register(typeof(LuaProvider));
            App.Instance.Register(typeof(EventProvider));
            App.Instance.Register(typeof(LuaDemo));
        }

    }

    /**
     * 这个类是入口类用于启动框架 
     */
    public class Program : UnityEngine.MonoBehaviour
    {
        public void Awake()
        {
            (new Application(this)).Bootstrap(new Type[] { typeof(Bootstraps) ,typeof(RegisterProvidersBootstrap) }).Init();

            gameObject.AddComponent<TestLuaBehaviour>();
        }
    }

}