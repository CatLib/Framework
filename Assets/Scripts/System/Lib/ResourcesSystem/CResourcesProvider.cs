using CatLib.Container;
using CatLib.Base;
using CatLib.Contracts.ResourcesSystem;


namespace CatLib.ResourcesSystem
{

    /// <summary>
    /// 资源服务提供商
    /// </summary>
    public class CResourcesProvider : CServiceProvider
    {

        public override void Register()
        {
            Application.Singleton<CResources>().Alias<IResources>();
        }

    }

}