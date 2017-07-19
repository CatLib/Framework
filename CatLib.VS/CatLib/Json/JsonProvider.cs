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

#if CATLIB
using CatLib.API;
using CatLib.API.Json;

namespace CatLib.Json
{
    /// <summary>
    /// Json 服务
    /// </summary>
    public sealed class JsonProvider : ServiceProvider
    {
        /// <summary>
        /// 当注册服务提供者
        /// </summary>
        public override void Register()
        {
            App.Singleton<JsonUtility>().OnResolving((binder, obj) =>
            {
                var jsonUtility = obj as JsonUtility;
                jsonUtility.SetJson(new SimpleJsonAdapter());
                return jsonUtility;
            }).Alias<IJson>().Alias<IJsonAware>().Alias("catlib.json.parse");
        }
    }
}
#endif