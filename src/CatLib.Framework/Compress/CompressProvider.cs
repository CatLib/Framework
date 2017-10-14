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
using CatLib.API.Compress;

namespace CatLib.Compress
{
    /// <summary>
    /// 压缩解压缩服务提供者
    /// </summary>
    public sealed class CompressProvider : IServiceProvider
    {
        /// <summary>
        /// 默认压缩等级
        /// </summary>
        public int DefaultLevel { get; set; }

        /// <summary>
        /// 压缩解压缩服务提供者
        /// </summary>
        public CompressProvider()
        {
            DefaultLevel = 6;
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
            App.Singleton<CompressManager>().Alias<ICompressManager>().OnResolving((_, obj) =>
            {
                var manager = (CompressManager)obj;
                manager.Extend(() => new GZipAdapter(DefaultLevel));
                manager.Extend(() => new LzmaAdapter(), "lzma");
                manager.Extend(() => manager.Get(), "gzip");
                return obj;
            });

            App.Singleton<ICompress>((_, __) => App.Make<ICompressManager>().Default);
        }
    }
}
#endif