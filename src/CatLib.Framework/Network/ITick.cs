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

namespace CatLib.Network
{
    /// <summary>
    /// 时间流逝
    /// </summary>
    public interface ITick
    {
        /// <summary>
        /// 滴答
        /// </summary>
        /// <param name="elapseSeconds">滴答间流逝的时间</param>
        void Tick(int elapseSeconds);
    }
}
