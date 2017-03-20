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
using CatLib.API.Routing;

namespace CatLib.Routing
{

    /// <summary>
    /// 路由参数绑定
    /// </summary>
    public class RouteParameterBinder
    {

        /// <summary>
        /// 获取参数
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static Dictionary<string, string> Parameters(Route route , IRequest request)
        {
            return null;
        }
    }

}