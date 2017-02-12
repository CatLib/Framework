using CatLib.Base;
using CatLib.NetPackge;
using CatLib.Container;
using CatLib.Contracts.NetPackage;

namespace CatLib.NetPackage
{

    public class CNetPackageProvider : CServiceProvider
    {
        public override void Register()
        {
            Application.Bind<CCatLibUnpacking>().Alias<IUnpacking>();
        }
    }

}