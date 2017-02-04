using UnityEngine;
using System.Collections;
using CatLib.Container;
using CatLib.Base;
using CatLib.Contracts.ResourcesSystem;
using CatLib.Contracts.Base;

namespace CatLib.ResourcesSystem
{

    /// <summary>
    /// 资源服务提供商
    /// </summary>
    public class CResourcesProvider : CServiceProvider
    {

        public CResourcesProvider(IApplication app) : base(app)
        {
        }

        public override void Register()
        {
            application.Singleton<CResources>().Alias<IResources>();
        }

    }

}