using UnityEngine;
using System.Collections;
using CatLib.Base;
using CatLib.Container;

namespace CatLib.Network
{

    /// <summary>
    /// 网络服务提供商
    /// </summary>
    public class CNetworkProvider : CServiceProvider
    {

        public CNetworkProvider(CApplication app) : base(app)
        {
        }


        public override void Register()
        {
            application.Singleton<CNetwork>();
        }


    }

}