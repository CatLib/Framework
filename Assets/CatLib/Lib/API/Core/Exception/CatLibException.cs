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
 
namespace CatLib.API
{
    /// <summary>
    /// CatLib异常
    /// </summary>
    public class CatLibException : System.Exception
    {
        /// <summary>
        /// CatLib异常
        /// </summary>
        /// <param name="message">异常消息</param>
        public CatLibException(string message) : base(message) { }
    }
}