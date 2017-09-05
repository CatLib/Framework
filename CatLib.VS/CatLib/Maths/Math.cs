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
using System.Collections.Generic;
using CatLib.API.Maths;

namespace CatLib.Maths
{
    /// <summary>
    /// 数学库
    /// </summary>
    public sealed class Math : IMath
    {
        /// <summary>
        /// 随机数算法构建器字典
        /// </summary>
        private readonly Dictionary<RandomTypes , Func<int, IRandom>> randoms = new Dictionary<RandomTypes, Func<int, IRandom>>();

        /// <summary>
        /// 生成随机算法
        /// </summary>
        /// <param name="type">算法类型</param>
        /// <returns>随机数算法</returns>
        public IRandom MakeRandom(RandomTypes type = RandomTypes.MersenneTwister)
        {
            return MakeRandom(Util.MakeSeed(), type);
        }

        /// <summary>
        /// 生成随机算法
        /// </summary>
        /// <returns>随机数算法</returns>
        public IRandom MakeRandom(int seed, RandomTypes type = RandomTypes.MersenneTwister)
        {
            Func<int, IRandom> builder;
            if (!randoms.TryGetValue(type, out builder))
            {
                ThrowRandomTypesNotImplemented(type);
            }
            var result = builder.Invoke(seed);
            if (result == null)
            {
                ThrowRandomTypesNotImplemented(type);
            }
            return result;
        }

        /// <summary>
        /// 注册随机数算法
        /// </summary>
        /// <param name="type">算法类型</param>
        /// <param name="builder">构建器</param>
        public void RegisterRandom(RandomTypes type, Func<int, IRandom> builder)
        {
            Guard.Requires<ArgumentNullException>(builder != null);
            randoms.Add(type, builder);
        }

        /// <summary>
        /// 触发随机算法没有实现异常
        /// </summary>
        /// <param name="type">随机算法</param>
        private void ThrowRandomTypesNotImplemented(RandomTypes type)
        {
            throw new NotImplementedException("RandomTypes [" + type + "] is not implemented");
        }
    }
}
