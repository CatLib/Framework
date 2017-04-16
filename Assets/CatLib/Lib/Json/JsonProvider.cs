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

using CatLib.API.Json;

namespace CatLib.Json
{
    /// <summary>
    /// json服务提供商
    /// </summary>
    public class JsonProvider : ServiceProvider
    {
        /// <summary>
        /// 注册json服务
        /// </summary>
        public override void Register()
        {
            RegisterParse();
            App.Singleton<Json>().Alias<IJson>().Alias("json");
        }

        /// <summary>
        /// 注册解析器
        /// </summary>
        protected void RegisterParse()
        {
            App.Singleton<IJsonAdapter>((app, param) => new TinyJsonAdapter()).Alias("json.parse");
        }
    }
}
