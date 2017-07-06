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
            App.Singleton<Translator>().Alias<ITranslator>().Alias("translation").OnResolving((bind, obj) =>
            {
                var config = App.Make<IConfigManager>();
                var tran = obj as Translator;
                tran.SetSelector(new Selector());

                if (config == null)
                {
                    return obj;
                }

                tran.SetLocale(config.Default.Get("translation.default", Languages.Chinese));
                tran.SetFallback(config.Default.Get("translation.fallback", Languages.Chinese));

                return obj;
            });
        }
    }
}