using CatLib.Base;
using CatLib.Container;
using CatLib.Contracts.NetPackage;
using CatLib.Contracts.Network;
using System;

namespace CatLib.Network
{

    /// <summary>
    /// 网络服务提供商
    /// </summary>
    public class CNetworkProvider : CServiceProvider
    {

        public override Type[] ProviderDepend
        {
            get { return new Type[] { typeof(IPacking) }; }
        }

        public override void Register()
        {
            App.Singleton<CNetwork>().Alias<INetwork>();
            App.Bind<CHttpWebRequest>().Alias<IConnectorHttp>();
            //Application.Bind<CWebRequest>().Alias<IConnectorHttp>();
            App.Bind<CTcpRequest>().Alias<IConnectorTcp>();
        }


    }

}