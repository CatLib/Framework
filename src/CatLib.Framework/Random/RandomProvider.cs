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

#if CATLIB
using CatLib.API.Random;
using CatLib._3rd.MathNet.Numerics.Random;

namespace CatLib.Random
{
    /// <summary>
    /// 随机算法服务提供者
    /// </summary>
    public sealed class RandomProvider : IServiceProvider
    {
        /// <summary>
        /// 默认的随机算法
        /// </summary>
        public string DefaultRandomType { get; set; }

        /// <summary>
        /// 随机算法服务提供者
        /// </summary>
        public RandomProvider()
        {
            DefaultRandomType = RandomTypes.MersenneTwister;
        }

        /// <summary>
        /// 服务提供者初始化
        /// </summary>
        public void Init()
        {
        }

        /// <summary>
        /// 当注册服务提供者
        /// </summary>
        public void Register()
        {
            App.Singleton<RandomFactory>((_, __) => new RandomFactory(DefaultRandomType))
                .Alias<IRandomFactory>().Alias<IRandom>().OnResolving((_, obj) =>
            {
                var math = (RandomFactory) obj;
                InitedRandom(math);
                return obj;
            });
        }

        /// <summary>
        /// 初始化随机库
        /// </summary>
        /// <param name="randomFactory">随机库</param>
        private void InitedRandom(RandomFactory randomFactory)
        {
            randomFactory.RegisterRandom(RandomTypes.MersenneTwister, (seed) => new RandomAdaptor(new MersenneTwister(seed)));
            randomFactory.RegisterRandom(RandomTypes.Xorshift, (seed) => new RandomAdaptor(new Xorshift(seed)));
            randomFactory.RegisterRandom(RandomTypes.WH2006, (seed) => new RandomAdaptor(new WH2006(seed)));
            randomFactory.RegisterRandom(RandomTypes.Mrg32k3a, (seed) => new RandomAdaptor(new Mrg32k3a(seed)));
        }
    }
}
#endif