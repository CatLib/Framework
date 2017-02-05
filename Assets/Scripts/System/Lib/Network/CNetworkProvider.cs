using UnityEngine;
using System.Collections;
using CatLib.Base;
using CatLib.Container;
using CatLib.Contracts.Network;
using CatLib.Contracts.Base;
using CatLib.Network.UnityWebRequest;

namespace CatLib.Network
{

    /// <summary>
    /// 网络服务提供商
    /// </summary>
    public class CNetworkProvider : CServiceProvider
    {

        public CNetworkProvider(IApplication app) : base(app)
        {
        }


        public override void Register()
        {
            application.Singleton<CNetwork>().Alias<INetwork>();
            application.Bind<CWebRequest>().Alias<IConnectorHttp>();
            application.Bind<CCookieWebRequest>().Alias("testcookie");
        }


    }

}