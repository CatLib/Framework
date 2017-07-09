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

using CatLib.Debugger.WebConsole;
using System.Collections.Generic;

namespace CatLib.Debugger.WebDebugger.Protocol
{
    /// <summary>
    /// 获取分组接口
    /// </summary>
    internal sealed class GetCatergroy : IWebConsoleResponse
    {
        /// <summary>
        /// 响应
        /// </summary>
        public object Response
        {
            get { return outputs; }
        }

        /// <summary>
        /// 输出
        /// </summary>
        private IList<string> outputs;

        /// <summary>
        /// 向Web控制台屏幕中输出一条消息
        /// </summary>
        /// <param name="categroyName">分组名</param>
        public void WriteLine(string categroyName)
        {
            outputs = null;
        }
    }
}
