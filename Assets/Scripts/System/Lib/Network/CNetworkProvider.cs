using CatLib.Base;
using CatLib.Container;
using CatLib.Contracts.Network;
using CatLib.Network.UnityWebRequest;
using CatLib.Network.HttpWebRequest;

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
            Application.Bind<CWebRequest>().Alias<IConnectorHttp>();
            Application.Bind<CHttpWebRequest>().Alias("testcookie");
        }


    }

}