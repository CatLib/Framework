using CatLib.Contracts.ResourcesSystem;


namespace CatLib.ResourcesSystem
{

    /// <summary>
    /// 资源服务提供商
    /// </summary>
    public class ResourcesProvider : ServiceProvider
    {

        public override void Register()
        {
            App.Singleton<Resources>().Alias<IResources>();
        }

    }

}