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

using CatLib.API.Event;

namespace CatLib.Event
{
    /// <summary>
    /// 事件服务
    /// </summary>
    public sealed class EventProvider : ServiceProvider
    {
        /// <summary>
        /// 注册事件服务
        /// </summary>
        public override void Register()
        {
            App.Bind<EventImpl>().Alias<IEventImpl>();
        }
    }
}