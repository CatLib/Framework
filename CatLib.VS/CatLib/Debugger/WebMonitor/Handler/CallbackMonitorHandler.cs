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
using CatLib.Stl;

namespace CatLib.Debugger.WebMonitor.Handler
{
    /// <summary>
    /// 回调获取监控值处理器
    /// </summary>
    public sealed class CallbackMonitorHandler : IMonitorHandler
    {
        /// <summary>
        /// 基础处理器
        /// </summary>
        private readonly IMonitorHandler baseHandler;

        /// <summary>
        /// 回调
        /// </summary>
        private readonly Func<object> callback;

        /// <summary>
        /// 分类
        /// </summary>
        public string[] Category
        {
            get
            {
                return baseHandler.Category;
            }
        }

        /// <summary>
        /// 监控的标题
        /// </summary>
        public string Title
        {
            get
            {
                return baseHandler.Title;
            }
        }

        /// <summary>
        /// 监控值的单位描述
        /// </summary>
        public string Unit
        {
            get
            {
                return baseHandler.Unit;
            }
        }

        /// <summary>
        /// 实时的监控值
        /// </summary>
        public string Value
        {
            get
            {
                baseHandler.Handler(callback.Invoke());
                return baseHandler.Value;
            }
        }

        /// <summary>
        /// 回调获取处理器
        /// </summary>
        /// <param name="baseHandler">基础处理器</param>
        /// <param name="callback">回调获取监控值</param>
        public CallbackMonitorHandler(IMonitorHandler baseHandler, Func<object> callback)
        {
            Guard.Requires<ArgumentNullException>(baseHandler != null);
            Guard.Requires<ArgumentNullException>(callback != null);
            this.baseHandler = baseHandler;
            this.callback = callback;
        }

        /// <summary>
        /// 处理句柄
        /// </summary>
        /// <param name="value">值</param>
        public void Handler(object value)
        {
        }
    }
}
