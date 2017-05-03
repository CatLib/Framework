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
using CatLib.IO;
using Store = CatLib.CsvStore;
using CatLib.Csv;
using CatLib.DataTable;
using CatLib.API.IO;

namespace CatLib.Demo.CsvStore
{

    /**
     * 为了demo演示需要强制覆盖全局配置（正常项目不要这样使用）
     */
    public class OverrideConfig : ServiceProvider
    {
        public override void Init()
        {
            //由于是demo需要所以强制更改默认路径
            Env env = App[typeof(Env).ToString()] as Env;
            env.SetResourcesBuildPath(Global.BasePath + "/CsvStore/Resources");
			env.SetResourcesNoBuildPath(Global.BasePath + "/CsvStore/Resources");
            env.SetDebugLevel(DebugLevels.Dev);

			//单纯由于是demo所以强制更改全局配置的路径，正常项目请不要这样用
			IIOFactory io = App.Make<IIOFactory>();
			Store.CsvStore csvStore = App[typeof(Store.CsvStore).ToString()] as Store.CsvStore;
			csvStore.SetDirctory(io.Disk().Directory(Global.BasePath + "/CsvStore/Resources"));
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
			App.Instance.Register(typeof(Store.CsvStoreProvider));
			App.Instance.Register(typeof(CsvParserProvider));
			App.Instance.Register(typeof(DataTableProvider));
			App.Instance.Register(typeof(IOProvider));
			App.Instance.Register(typeof(CsvStoreDemo));
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