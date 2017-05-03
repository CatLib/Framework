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
    /// 域异常
    /// </summary>
    public class DomainException : CatLibException
    {
        /// <summary>
        /// 创建一个域异常
        /// </summary>
        /// <param name="message">错误描述</param>
        public DomainException(string message) : base(message) { }
    }
}
