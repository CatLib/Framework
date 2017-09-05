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

using CatLib.API.Maths;
using MathNet.Numerics.Random;

namespace CatLib.Maths
{
    /// <summary>
    /// 数学库服务提供者
    /// </summary>
    public sealed class MathProvider : IServiceProvider
    {
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
            App.Singleton<Math>().Alias<IMath>().OnResolving((_, obj) =>
            {
                var math = (Math) obj;
                InitedRandom(math);
                return obj;
            });
        }

        /// <summary>
        /// 初始化随机库
        /// </summary>
        /// <param name="math">随机库</param>
        private void InitedRandom(Math math)
        {
            math.RegisterRandom(RandomTypes.MersenneTwister, (seed) => new RandomAdaptor(new MersenneTwister(seed)));
            math.RegisterRandom(RandomTypes.Xorshift, (seed) => new RandomAdaptor(new Xorshift(seed)));
        }
    }
}
