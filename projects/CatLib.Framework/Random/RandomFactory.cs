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
using CatLib.API.Random;

namespace CatLib.Random
{
    /// <summary>
    /// 随机算法生成器
    /// </summary>
    public sealed class RandomFactory : IRandomFactory
    {
        /// <summary>
        /// 随机数算法构建器字典
        /// </summary>
        private readonly Dictionary<RandomTypes, Func<int, IRandom>> randomsMaker = new Dictionary<RandomTypes, Func<int, IRandom>>();

        /// <summary>
        /// 随机算法实例缓存
        /// </summary>
        private readonly Dictionary<RandomTypes, IRandom> randomsCache = new Dictionary<RandomTypes, IRandom>();

        /// <summary>
        /// 默认的随机算法类型
        /// </summary>
        private readonly RandomTypes defaultRandomType;

        /// <summary>
        /// 构造一个随机算法生成器
        /// </summary>
        /// <param name="defaultType">默认的随机算法类型</param>
        public RandomFactory(RandomTypes defaultType)
        {
            defaultRandomType = defaultType;
        }

        /// <summary>
        /// 生成随机算法
        /// </summary>
        /// <returns>随机数算法</returns>
        public IRandom Make()
        {
            return Make(defaultRandomType);
        }

        /// <summary>
        /// 生成随机算法
        /// </summary>
        /// <param name="type">算法类型</param>
        /// <returns>随机数算法</returns>
        public IRandom Make(RandomTypes type)
        {
            return Make(Util.MakeSeed(), type);
        }

        /// <summary>
        /// 生成随机算法
        /// </summary>
        /// <returns>随机数算法</returns>
        public IRandom Make(int seed, RandomTypes type)
        {
            Func<int, IRandom> builder;
            IRandom result;

            if (!randomsMaker.TryGetValue(type, out builder) || 
                 (result = builder.Invoke(seed)) == null)
            {
                throw new NotImplementedException("RandomTypes [" + type + "] is not implemented");
            }

            return result;
        }

        /// <summary>
        /// 返回一个随机数
        /// </summary>
        /// <returns>随机数</returns>
        public int Next()
        {
            return Next(defaultRandomType);
        }

        /// <summary>
        /// 返回一个随机数
        /// </summary>
        /// <param name="type">使用的随机算法类型</param>
        /// <returns>随机数</returns>
        public int Next(RandomTypes type)
        {
            var random = GetRandom(type);
            return random.Next();
        }

        /// <summary>
        /// 返回一个随机数
        /// </summary>
        /// <param name="maxValue">最大值</param>
        /// <returns>随机数</returns>
        public int Next(int maxValue)
        {
            return Next(maxValue, defaultRandomType);
        }

        /// <summary>
        /// 返回一个随机数
        /// </summary>
        /// <param name="maxValue">最大值</param>
        /// <param name="type">使用的随机算法类型</param>
        /// <returns>随机数</returns>
        public int Next(int maxValue, RandomTypes type)
        {
            var random = GetRandom(type);
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
            return Next(minValue, maxValue, defaultRandomType);
        }

        /// <summary>
        /// 返回一个随机数
        /// </summary>
        /// <param name="minValue">最小值</param>
        /// <param name="maxValue">最大值</param>
        /// <param name="type">使用的随机算法类型</param>
        /// <returns>随机数</returns>
        public int Next(int minValue, int maxValue, RandomTypes type)
        {
            var random = GetRandom(type);
            return random.Next(minValue, maxValue);
        }

        /// <summary>
        /// 生成随机数填充流
        /// </summary>
        /// <param name="buffer">流</param>
        public void NextBytes(byte[] buffer)
        {
            NextBytes(buffer, defaultRandomType);
        }

        /// <summary>
        /// 生成随机数填充流
        /// </summary>
        /// <param name="buffer">流</param>
        /// <param name="type">使用的随机算法类型</param>
        public void NextBytes(byte[] buffer, RandomTypes type)
        {
            var random = GetRandom(type);
            random.NextBytes(buffer);
        }

        /// <summary>
        /// 返回一个介于0到1之间的随机数
        /// </summary>
        /// <returns>随机数</returns>
        public double NextDouble()
        {
            return NextDouble(defaultRandomType);
        }

        /// <summary>
        /// 返回一个介于0到1之间的随机数
        /// </summary>
        /// <param name="type">使用的随机算法类型</param>
        /// <returns>随机数</returns>
        public double NextDouble(RandomTypes type)
        {
            var random = GetRandom(type);
            return random.NextDouble();
        }

        /// <summary>
        /// 注册随机数算法
        /// </summary>
        /// <param name="type">算法类型</param>
        /// <param name="builder">构建器</param>
        public void RegisterRandom(RandomTypes type, Func<int, IRandom> builder)
        {
            Guard.Requires<ArgumentNullException>(type != null);
            Guard.Requires<ArgumentNullException>(builder != null);
            randomsMaker.Add(type, builder);
        }

        /// <summary>
        /// 获取随机数算法
        /// </summary>
        /// <param name="type">算法类型</param>
        /// <returns>随机数算法</returns>
        private IRandom GetRandom(RandomTypes type)
        {
            IRandom random;
            if (!randomsCache.TryGetValue(type, out random))
            {
                randomsCache[type] = random = Make(type);
            }
            return random;
        }
    }
}
