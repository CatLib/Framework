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

using CatLib.API.Protobuf;

namespace CatLib.Protobuf
{
    /// <summary>
    /// Protobuf服务提供商
    /// </summary>
    public sealed class ProtobufProvider : ServiceProvider
    {
        /// <summary>
        /// 注册Protobuf服务
        /// </summary>
        public override void Register()
        {
            RegisterProtobufAdapter();
            App.Singleton<Protobuf>().Alias<IProtobuf>().Alias("protobuf");
        }

        /// <summary>
        /// 注册适配器
        /// </summary>
        private void RegisterProtobufAdapter()
        {
            App.Singleton<ProtobufNetAdapter>().Alias<IProtobufAdapter>();
        }
    }
}