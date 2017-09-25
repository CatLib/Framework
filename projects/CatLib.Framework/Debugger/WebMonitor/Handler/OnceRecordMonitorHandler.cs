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

namespace CatLib.Debugger.WebMonitor.Handler
{
    /// <summary>
    /// 监控处理器
    /// </summary>
    public sealed class OnceRecordMonitorHandler : IMonitorHandler
    {
        /// <summary>
        /// 标签
        /// </summary>
        public string[] Tags { get; private set; }

        /// <summary>
        /// 监控的名字
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 监控值的单位描述
        /// </summary>
        public string Unit { get; private set; }

        /// <summary>
        /// 实时的监控值
        /// </summary>
        public string Value
        {
            get
            {
                return callback.Invoke().ToString();
            }
        }

        /// <summary>
        /// 回调获取结果
        /// </summary>
        private readonly Func<object> callback;

        /// <summary>
        /// 单次记录监控处理器
        /// </summary>
        /// <param name="name">标题</param>
        /// <param name="unit">单位值</param>
        /// <param name="tags">tags</param>
        /// <param name="callback">回调</param>
        public OnceRecordMonitorHandler(string name, string unit , string[] tags , Func<object> callback)
        {
            Guard.NotEmptyOrNull(name, "name");
            Guard.Requires<ArgumentNullException>(unit != null);
            Guard.Requires<ArgumentNullException>(tags != null);
            Guard.Requires<ArgumentNullException>(callback != null);
            Name = name;
            Unit = unit;
            Tags = tags;
            this.callback = callback;
        }
    }
}
