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
using CatLib.API.Json;

namespace CatLib.Json
{
    /// <summary>
    /// Json 服务
    /// </summary>
    public sealed class JsonProvider : IServiceProvider
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
        }

        /// <summary>
        /// 当注册服务提供者
        /// </summary>
        public void Register()
        {
            App.Singleton<JsonUtility>().OnResolving((binder, obj) =>
            {
                var jsonUtility = (JsonUtility)obj;
                jsonUtility.SetJson(new SimpleJsonAdapter());
                return jsonUtility;
            }).Alias<IJson>().Alias<IJsonAware>();
        }
    }
}
#endif