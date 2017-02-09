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

        public override void Register()
        {
            Application.Singleton<CNetwork>().Alias<INetwork>();
            Application.Bind<CWebRequest>().Alias<IConnectorHttp>();
            Application.Bind<CCookieWebRequest>().Alias("testcookie");
        }


    }

}