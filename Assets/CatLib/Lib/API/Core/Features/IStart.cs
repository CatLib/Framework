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
    /// 当启动时
    /// </summary>
    public interface IStart
    {
        /// <summary>
        /// 启动时触发
        /// </summary>
        void Start();
    }
}
