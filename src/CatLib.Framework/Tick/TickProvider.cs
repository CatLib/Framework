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

namespace CatLib.Tick
{
    /// <summary>
    /// 时间摆钟服务提供者
    /// </summary>
    public sealed class TickProvider : IServiceProvider
    {
        /// <summary>
        /// 初始化服务提供者
        /// </summary>
        [Priority(5)]
        public void Init()
        {
            App.Make<TimeTicker>();
        }

        /// <summary>
        /// 注册服务提供者
        /// </summary>
        public void Register()
        {
            App.Singleton<TimeTicker>();
        }
    }
}
