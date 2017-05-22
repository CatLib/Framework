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

namespace CatLib
{
    /// <summary>
    /// 注册服务提供商的引导程序
    /// </summary>
    public class RegisterProvidersBootstrap : IBootstrap
    {
        /// <summary>
        /// 引导程序
        /// </summary>
        public void Bootstrap()
        {
            foreach (var t in Providers.ServiceProviders)
            {
                App.Instance.Register(t);
            }
        }
    }
}