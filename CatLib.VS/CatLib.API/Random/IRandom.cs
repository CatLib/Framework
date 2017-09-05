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

namespace CatLib.API.Random
{
    /// <summary>
    /// 随机算法
    /// </summary>
    public interface IRandom
    {
        /// <summary>
        /// 返回一个随机数
        /// </summary>
        /// <returns>随机数</returns>
        int Next();

        /// <summary>
        /// 返回一个随机数
        /// </summary>
        /// <param name="minValue">最小值</param>
        /// <returns>随机数</returns>
        int Next(int minValue);

        /// <summary>
        /// 返回一个随机数
        /// </summary>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <returns>随机数</returns>
        int Next(int minValue, int maxValue);

        /// <summary>
        /// 生成随机数填充流
        /// </summary>
        /// <param name="buffer">流</param>
        void NextBytes(byte[] buffer);

        /// <summary>
        /// 返回一个介于0到1之间的随机数
        /// </summary>
        /// <returns>随机数</returns>
        double NextDouble();
    }
}
