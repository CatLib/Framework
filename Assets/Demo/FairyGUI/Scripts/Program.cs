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
using CatLib.Resources;
using CatLib.IO;
using CatLib.FairyGUI;
using CatLib.Event;
using CatLib.Flux;
using CatLib.Time;

namespace CatLib.Demo.FairyGUI
{

    /**
     * 为了demo演示需要强制覆盖全局配置
     */
    public class OverrideConfig : ServiceProvider
    {
        public override void Init()
        {
            Env env = App[typeof(Env).ToString()] as Env;
            env.SetResourcesBuildPath(Global.BasePath + "/FairyGUI/Resources");
            env.SetDebugLevel(DebugLevels.Dev);
            env.SetDebugLevel(DebugLevels.Staging); //使用staging从ab包中读取
        }

        public override void Register() { }
    }

    /**
     * 这个类提供了当前demo演示时用到的组件 
     */
    public class Bootstraps : IBootstrap
    {

        public void Bootstrap()
        {
            App.Instance.Register(typeof(EventProvider));
            App.Instance.Register(typeof(ResourcesProvider));
            App.Instance.Register(typeof(CoreProvider));
            App.Instance.Register(typeof(IOProvider));
            App.Instance.Register(typeof(FluxProvider));
            App.Instance.Register(typeof(TimeProvider));
            App.Instance.Register(typeof(FairyGUIProvider));
            App.Instance.Register(typeof(FairyGUIDemo));
            App.Instance.Register(typeof(OverrideConfig));
        }

    }

    /**
     * 这个类是入口类用于启动框架 
     */
    public class Program : UnityEngine.MonoBehaviour
    {
        public void Awake()
        {
            (new Application(this)).Bootstrap(new Type[] { typeof(Bootstraps) }).Init();
        }
    }

}