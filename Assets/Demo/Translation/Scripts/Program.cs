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
using MonoBehaviour = UnityEngine.MonoBehaviour;
using CatLib.API;
using CatLib.Event;
using CatLib.Translation;
using CatLib.INI;
using CatLib.IO;

namespace CatLib.Demo.Translation
{

    /**
     * 为了demo演示需要强制覆盖全局配置（正常项目不要这样使用）
     */
    public class OverrideConfig : ServiceProvider
    {
        public override void Init()
        {
            //由于是demo需要所以强制更改默认路径
            Env env = App[typeof(Env)] as Env;
            env.SetResourcesBuildPath(Global.BasePath + "/Translation/Resources");
			env.SetResourcesNoBuildPath(Global.BasePath + "/Translation/Resources");
            env.SetDebugLevel(DebugLevels.Dev);

            //通过配置可以指定翻译文件所在位置（这一步正常应该在配置中配置，但是由于是demo所以我们显式声明）
            Translator tran = App[typeof(Translator)] as Translator;
            tran.SetRoot("lang");
            tran.SetFallback("zh");
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
            App.Instance.Register(typeof(CoreProvider));
			App.Instance.Register(typeof(TranslationProvider));
			App.Instance.Register(typeof(INIProvider));
			App.Instance.Register(typeof(IOProvider));
			App.Instance.Register(typeof(TranslationDemo));
            App.Instance.Register(typeof(OverrideConfig));
        }

    }

    /**
     * 这个类是入口类用于启动框架 
     */
    public class Program : MonoBehaviour
    {
        public void Awake()
        {
            (new Application(this)).Bootstrap(new Type[] { typeof(Bootstraps) }).Init();
        }
    }

}