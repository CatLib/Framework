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

using System;
using CatLib.API.Random;

namespace CatLib.Random
{
    /// <summary>
    /// 随机算法适配器
    /// </summary>
    public sealed class RandomAdaptor : IRandom
    {
        /// <summary>
        /// 随机算法
        /// </summary>
        private readonly System.Random random;

        /// <summary>
        /// 随机算法适配器
        /// </summary>
        /// <param name="random">随机算法</param>
        public RandomAdaptor(System.Random random)
        {
            Guard.Requires<ArgumentNullException>(random != null);
            this.random = random;
        }

        /// <summary>
        /// 返回一个随机数
        /// </summary>
        /// <returns>随机数</returns>
        public int Next()
        {
            return random.Next();
        }

        /// <summary>
        /// 返回一个随机数
        /// </summary>
        /// <param name="maxValue">最小值</param>
        /// <returns>随机数</returns>
        public int Next(int maxValue)
        {
            return random.Next(maxValue);
        }

        /// <summary>
        /// 返回一个随机数
        /// </summary>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <returns>随机数</returns>
        public int Next(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }

        /// <summary>
        /// 生成随机数填充流
        /// </summary>
        /// <param name="buffer">流</param>
        public void NextBytes(byte[] buffer)
        {
            random.NextBytes(buffer);
        }

        /// <summary>
        /// 返回一个介于0到1之间的随机数
        /// </summary>
        /// <returns>随机数</returns>
        public double NextDouble()
        {
            return random.NextDouble();
        }
    }
}
