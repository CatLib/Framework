/*
 * This file is part of the CatLib package.
 *
 * (c) Ming ming <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using CatLib.API.MsgPack;

namespace CatLib.MsgPack
{
    public class MsgPackProvider : ServiceProvider
    {
        /// <summary>
        /// 注册Protobuf服务
        /// </summary>
        public override void Register()
        {
            RegisterProtobufAdapter();
            App.Singleton<MsgPack>().Alias<IMsgPack>().Alias("msgpack");
        }

        /// <summary>
        /// 注册适配器
        /// </summary>
        private void RegisterProtobufAdapter()
        {
            App.Singleton<MsgPackAdapter>().Alias<IMsgPackAdapter>();
        }
    }
}


