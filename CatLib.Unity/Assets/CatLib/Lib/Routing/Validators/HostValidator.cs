﻿/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using System.Text.RegularExpressions;

namespace CatLib.Routing
{
    /// <summary>
    /// Host验证器
    /// </summary>
    internal class HostValidator : IValidators
    {
        /// <summary>
        /// 是否匹配
        /// </summary>
        /// <param name="route">路由条目</param>
        /// <param name="request">请求</param>
        /// <returns>是否匹配</returns>
        public bool Matches(Route route, Request request)
        {
            return string.IsNullOrEmpty(route.Compiled.HostRegex) || (new Regex(route.Compiled.HostRegex)).IsMatch(request.RouteUri.Host);
        }
    }
}
