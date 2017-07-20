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

using System;

namespace CatLib.Debugger.WebMonitorContent
{
    /// <summary>
    /// 监控工厂
    /// </summary>
    public sealed class MonitorFactory
    {
        /// <summary>
        /// 定义一个监控
        /// </summary>
        /// <param name="key"></param>
        /// <param name="title"></param>
        /// <param name="unit"></param>
        /// <param name="callback"></param>
        public void Monitor(string key, string title, string unit, Func<object> callback)
        {
            
        }
    }
}
