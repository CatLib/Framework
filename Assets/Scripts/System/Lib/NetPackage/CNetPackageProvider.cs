using CatLib.Base;
using CatLib.Container;
using CatLib.Contracts.Network;

namespace CatLib.NetPackage
{

    public class CNetPackageProvider : CServiceProvider
    {
        public override void Register()
        {
            App.Bind<CBasePackage>().Alias<IPackage>().Alias("network.package.base");
            App.Bind<CTextProtocol>().Alias<IProtocol>().Alias("network.protocol.text");

            App.Bind<CFramePacking>().Alias<IPacking>().Alias("network.packing.frame");
            App.Bind<CTextPacking>().Alias("network.packing.text");
        }
    }

}