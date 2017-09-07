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

using CatLib.API.Routing;

namespace CatLib.Routing
{
    /// <summary>
    /// 响应
    /// </summary>
    internal sealed class Response : IResponse
    {
        /// <summary>
        /// 上下文
        /// </summary>
        private object context;

        /// <summary>
        /// 设定上下文
        /// </summary>
        /// <returns>上下文</returns>
        public object GetContext()
        {
            return context;
        }

        /// <summary>
        /// 设定上下文
        /// </summary>
        /// <param name="context">上下文</param>
        public void SetContext(object context)
        {
            this.context = context;
        }
    }
}