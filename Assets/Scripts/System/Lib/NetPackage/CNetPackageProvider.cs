using CatLib.Base;
using CatLib.Container;
using CatLib.Contracts.NetPackage;

namespace CatLib.NetPackage
{

    public class CNetPackageProvider : CServiceProvider
    {
        public override void Register()
        {
            App.Bind<CTextProtocol>().Alias<IProtocol>().Alias("protocol.text");
            App.Bind<CCatLibFramePacking>().Alias<IPacking>().Alias("frame");
        }
    }

}