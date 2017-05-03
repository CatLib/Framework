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

using CatLib.API.TimeQueue;

namespace CatLib.TimeQueue
{
    /// <summary>
    /// 时间队列服务提供商
    /// </summary>
    public sealed class TimeQueueProvider : ServiceProvider
    {
        /// <summary>
        /// 注册时间队列服务
        /// </summary>
        public override void Register()
        {
            App.Bind<TimeQueue>((app, param) => app.Make<TimeRunner>().CreateQueue()).Alias<ITimeQueue>();
            App.Singleton<TimeRunner>();
        }
    }

}