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

namespace CatLib
{
    /// <summary>
    /// 滴答
    /// </summary>
    public interface ITick
    {
        /// <summary>
        /// 滴答间流逝的时间
        /// </summary>
        /// <param name="elapseMillisecond">滴答间流逝的时间(MS)</param>
        void Tick(int elapseMillisecond);
    }
}
