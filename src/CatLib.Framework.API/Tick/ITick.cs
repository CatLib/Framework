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

namespace CatLib.API.Tick
{
    /// <summary>
    /// 滴答
    /// </summary>
    public interface ITick
    {
        /// <summary>
        /// 滴答间流逝的时间
        /// </summary>
        /// <param name="elapseSeconds">完成最后一帧所花费的时间（秒）</param>
        void Tick(float elapseSeconds);
    }
}
