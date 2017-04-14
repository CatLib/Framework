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
    /// 异常事件
    /// </summary>
    public class ExceptionEventArgs : System.EventArgs
    {
        /// <summary>
        /// 异常
        /// </summary>
        public System.Exception Exception { get; protected set; }

        public ExceptionEventArgs(System.Exception ex)
        {
            Exception = ex;
        }

    }

}