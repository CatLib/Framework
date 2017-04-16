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

namespace CatLib.API.Routing
{
    /// <summary>
    /// 响应
    /// </summary>
    public interface IResponse
    {
        /// <summary>
        /// 获取上下文
        /// </summary>
        /// <returns>上下文</returns>
        object GetContext();

        /// <summary>
        /// 设定上下文
        /// </summary>
        /// <param name="context"></param>
        void SetContext(object context);
    }
}