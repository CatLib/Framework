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

namespace CatLib.Routing
{
    /// <summary>
    /// 统一资源定位符验证器
    /// </summary>
    internal sealed class UriValidator : IValidators
    {
        /// <summary>
        /// 是否匹配
        /// </summary>
        /// <param name="route">路由条目</param>
        /// <param name="request">请求</param>
        /// <returns>是否匹配</returns>
        public bool Matches(Route route, Request request)
        {
            return route.Compiled.RouteRegex.IsMatch(request.RouteUri.NoParamFullPath);
        }
    }
}