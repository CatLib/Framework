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

using System.Collections;
using CatLib.API;
using CatLib.API.Config;
using CatLib.API.INI;
using CatLib.API.IO;
using CatLib.API.Translation;

namespace CatLib.Translation
{
    /// <summary>
    /// 国际化服务提供商
    /// </summary>
    public sealed class TranslationProvider : ServiceProvider
    {
        /// <summary>
        /// 当注册国际化服务时
        /// </summary>
        public override void Register()
        {
            RegisterLoader();
            RegisterSelector();

            App.Singleton<Translator>().Alias<ITranslator>().Alias("translation").OnResolving(obj =>
            {
                var config = App.Make<IConfigStore>();
                var tran = obj as Translator;

                var loader = App.Make("translation.loader") as IFileLoader;
                var selector = App.Make("translation.selector") as ISelector;

                tran.SetFileLoader(loader);
                tran.SetSelector(selector);

                if (config == null)
                {
                    return obj;
                }

                tran.SetLocale(config.Get(typeof(Translator), "default", "zh"));
                tran.SetRoot(config.Get(typeof(Translator), "root", null));
                tran.SetFallback(config.Get(typeof(Translator), "fallback", null));

                return obj;
            });
        }

        /// <summary>
        /// 注册消息选择器
        /// </summary>
        private void RegisterSelector()
        {
            App.Singleton("translation.selector", (app, param) => new MessageSelector());
        }

        /// <summary>
        /// 注册文件加载器
        /// </summary>
        private void RegisterLoader()
        {
            App.Singleton("translation.loader", (app, param) =>
            {
                var env = app.Make<IEnv>();
                var factory = app.Make<IIOFactory>();
                var disk = factory.Disk();

#if UNITY_EDITOR
                if (env.DebugLevel == DebugLevels.Auto || env.DebugLevel == DebugLevels.Dev)
                {
                    disk.SetConfig(new Hashtable{

                        {"root" , env.AssetPath + env.ResourcesNoBuildPath}

                    });
                }
#endif

                return new FileLoader(disk, app.Make<IIniLoader>());
            });
        }
    }
}