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

using CatLib.API.Thread;

namespace CatLib.Thread
{
    /// <summary>
    /// 线程服务提供商
    /// </summary>
    public sealed class ThreadProvider : ServiceProvider
    {
        /// <summary>
        /// 注册线程服务
        /// </summary>
        public override void Register()
        {
            App.Singleton<ThreadRuner>().Alias<IThread>();
        }
    }
}