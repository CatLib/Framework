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
            RegisterSelector();

            App.Singleton<Translator>().Alias<ITranslator>().Alias("translation").OnResolving((bind, obj) =>
            {
                var config = App.Make<IConfigManager>();
                var tran = obj as Translator;

                var selector = App.Make("translation.selector") as ISelector;

                tran.SetSelector(selector);

                if (config == null)
                {
                    return obj;
                }

                tran.SetLocale(config.Default.Get("translation.default", "zh"));
                tran.SetFallback(config.Default.Get("translation.fallback", string.Empty));

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
    }
}