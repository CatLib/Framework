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

using System.Collections.Generic;
using CatLib.API;

namespace CatLib.Bootstrap
{
    /// <summary>
    /// 初始配置
    /// 初始配置用于配置服务提供者在初始化时所需要的配置。
    /// 常规配置请不要在这里配置。
    /// </summary>
    public sealed class Configs
    {
        /// <summary>
        /// 初始配置
        /// </summary>
        public static IDictionary<string, object> ConfigsMap
        {
            get
            {
                return new Dictionary<string, object>
                {
                    { "env.debug" , DebugLevels.Auto },
                    { "env.asset.path" , "" },
                    { "routing.stripping.reserved" , "" }
                };
            }
        }
    }
}