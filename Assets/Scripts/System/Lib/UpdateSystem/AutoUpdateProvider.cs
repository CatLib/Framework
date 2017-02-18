using CatLib.Contracts.UpdateSystem;
using System.Collections;

namespace CatLib.UpdateSystem
{
    /// <summary>
    /// 自动更新服务提供商
    /// </summary>
    public class AutoUpdateProvider : ServiceProvider
    {

        public override ProviderProcess ProviderProcess
        {
            get
            {
                return ProviderProcess.AUTO_UPDATE;
            }
        }

        public override IEnumerator OnProviderProcess()
        {
            yield return App.Make<IAutoUpdate>().UpdateAsset();
        }

        public override void Register()
        {
            App.Singleton<AutoUpdate>().Alias<IAutoUpdate>();
        }

    }

}