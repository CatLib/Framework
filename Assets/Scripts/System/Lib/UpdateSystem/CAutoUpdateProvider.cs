using CatLib.Container;
using CatLib.Base;
using CatLib.Contracts.UpdateSystem;
using CapLib.Base;

namespace CatLib.UpdateSystem
{
    /// <summary>
    /// 自动更新服务提供商
    /// </summary>
    public class CAutoUpdateProvider : CServiceProvider
    {

        public override void Init() {

            Application.Event.One(CApplication.Events.ON_INITED_CALLBACK, (sender, e) =>
            {

                Application.Make<IAutoUpdate>().UpdateAsset();

            });

        }

        public override void Register()
        {
            Application.Singleton<CAutoUpdate>().Alias<IAutoUpdate>();
        }

    }

}