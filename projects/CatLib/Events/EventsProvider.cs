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
using CatLib.API.Events;

namespace CatLib.Events
{
    /// <summary>
    /// 事件服务提供者
    /// </summary>
    public sealed class EventsProvider : IServiceProvider
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
        }

        /// <summary>
        /// 注册事件服务
        /// </summary>
        public void Register()
        {
            App.Singleton<Dispatcher>().Alias<IDispatcher>();
        }
    }
}
#endif