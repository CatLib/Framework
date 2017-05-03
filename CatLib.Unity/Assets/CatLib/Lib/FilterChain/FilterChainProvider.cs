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

using CatLib.API.FilterChain;

namespace CatLib.FilterChain
{
    /// <summary>
    /// 过滤器链服务
    /// </summary>
    public class FilterChainProvider : ServiceProvider
    {
        /// <summary>
        /// 注册过滤器链服务
        /// </summary>
        public override void Register()
        {
            App.Singleton<FilterChain>().Alias<IFilterChain>();
        }
    }
}