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
 
using CatLib.API.Network;

namespace CatLib.NetPackage
{

    public class NetPackageProvider : ServiceProvider
    {
        public override void Register()
        {
            App.Bind<BasePackage>().Alias<IPackage>().Alias("network.package.base");

            App.Bind<ByteProtocol>().Alias<IProtocol>().Alias("network.protocol.byte");
            App.Bind<TextProtocol>().Alias("network.protocol.text");
            

            App.Bind<FramePacking>().Alias<IPacking>().Alias("network.packing.frame");
            App.Bind<TextPacking>().Alias("network.packing.text");
        }
    }

}