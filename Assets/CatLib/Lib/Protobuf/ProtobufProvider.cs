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

    public class ProtobufProvider : ServiceProvider
    {
        public override void Register()
        {
            RegisterProtobufAdapter();
            App.Singleton<Protobuf>().Alias<IProtobuf>().Alias("protobuf");
        }

        protected void RegisterProtobufAdapter()
        {
            App.Singleton<ProtobufNetAdapter>().Alias<IProtobufAdapter>();
        }
    }

}