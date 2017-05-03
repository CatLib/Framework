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

using CatLib.API.Compress;

namespace CatLib.Compress
{
    /// <summary>
    /// 压缩服务提供商
    /// </summary>
    public sealed class CompressProvider : ServiceProvider
    {
        /// <summary>
        /// 注册压缩服务
        /// </summary>
        public override void Register()
        {
            RegisterParse();
            App.Singleton<CompressService>().Alias<ICompress>().Alias("compress");
        }

        /// <summary>
        /// 注册解析器
        /// </summary>
        private void RegisterParse()
        {
            App.Singleton<ICompressAdapter>((app, param) => new ShareZipLibAdapter()).Alias("compress.parse");
        }
    }
}