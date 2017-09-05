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
    public enum RandomTypes
    {
        /// <summary>
        /// 马特赛特旋转演算法
        /// </summary>
        MersenneTwister,

        /// <summary>
        /// Xorshift
        /// </summary>
        Xorshift,

        /// <summary>
        /// Wichmann-Hill
        /// </summary>
        WH2006,

        /// <summary>
        /// 均匀随机数发生器(产数效率较低)
        /// </summary>
        Mrg32k3a,
    }
}
