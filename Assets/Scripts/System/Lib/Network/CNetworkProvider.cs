using CatLib.Base;
using CatLib.Container;
using CatLib.Contracts.Network;

namespace CatLib.Network
{

    /// <summary>
    /// 网络服务提供商
    /// </summary>
    public class CNetworkProvider : CServiceProvider
    {

        public override void Register()
        {
            Application.Singleton<CNetwork>().Alias<INetwork>();
            Application.Bind<CHttpWebRequest>().Alias<IConnectorHttp>();
            //Application.Bind<CWebRequest>().Alias<IConnectorHttp>();
            Application.Bind<CTcpRequest>().Alias<IConnectorTcp>();
        }


    }

}