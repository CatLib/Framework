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
    /// 随机算法类型
    /// </summary>
    public class RandomTypes : Enum
    {
        /// <summary>
        /// 马特赛特旋转演算法
        /// </summary>
        public static readonly RandomTypes MersenneTwister = new RandomTypes("MersenneTwister");

        /// <summary>
        /// Xorshift
        /// </summary>
        public static readonly RandomTypes Xorshift = new RandomTypes("Xorshift");

        /// <summary>
        /// Wichmann-Hill
        /// </summary>
        public static readonly RandomTypes WH2006 = new RandomTypes("WH2006");

        /// <summary>
        /// 均匀随机数发生器(产数效率较低)
        /// </summary>
        public static readonly RandomTypes Mrg32k3a = new RandomTypes("Mrg32k3a");

        /// <summary>
        /// 随机算法类型
        /// </summary>
        /// <param name="name">随机算法名字</param>
        protected RandomTypes(string name) : base(name)
        {
        }

        /// <summary>
        /// 字符串转RandomTypes
        /// </summary>
        /// <param name="type">类型</param>
        public static implicit operator RandomTypes(string type)
        {
            return new RandomTypes(type);
        }
    }
}
