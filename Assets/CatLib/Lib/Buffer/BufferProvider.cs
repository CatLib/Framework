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

using CatLib.API.Buffer;

namespace CatLib.Buffer
{
    /// <summary>
    /// buffer构建器服务提供商
    /// </summary>
    public class BufferProvider : ServiceProvider
    {
        /// <summary>
        /// 注册buffer构建器
        /// </summary>
        public override void Register()
        {
            App.Bind<BufferBuilder>().Alias<IBufferBuilder>().Alias("buffer");
        }
    }
}