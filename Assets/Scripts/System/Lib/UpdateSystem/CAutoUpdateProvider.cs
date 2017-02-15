using CatLib.Container;
using CatLib.Base;
using CatLib.Contracts.UpdateSystem;
using System.Collections;

namespace CatLib.UpdateSystem
{
    /// <summary>
    /// 自动更新服务提供商
    /// </summary>
    public class CAutoUpdateProvider : CServiceProvider
    {

        public override EProviderProcess ProviderProcess
        {
            get
            {
                return EProviderProcess.AUTO_UPDATE;
            }
        }

        public override IEnumerator OnProviderProcess()
        {
            yield return App.Make<IAutoUpdate>().UpdateAsset();
        }

        public override void Register()
        {
            App.Singleton<CAutoUpdate>().Alias<IAutoUpdate>();
        }

    }

}