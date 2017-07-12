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

using CatLib.Debugger.WebMonitor;
using CatLib.Debugger.WebMonitor.Handler;

namespace CatLib.Debugger.WebMonitorContent
{
    /// <summary>
    /// 堆内存
    /// </summary>
    public sealed class HeapMemory
    {
        /// <summary>
        /// 构建一个堆内存监控
        /// </summary>
        /// <param name="monitor">监控</param>
        public HeapMemory([Inject(Required = true)]IMonitor monitor)
        {
            monitor.DefinedMoitor("heap.memory",
                new CallbackMonitorHandler(new SizeMonitorHandler("HeapMemory"), GetHeapMemory));
        }

        /// <summary>
        /// 获取堆内存
        /// </summary>
        /// <returns>堆内存</returns>
        private object GetHeapMemory()
        {
            return 0;
        }
    }
}
