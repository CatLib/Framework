using UnityEngine;
using CatLib.Container;

namespace CatLib.UpdateSystem
{
    /// <summary>
    /// 自动更新服务提供商
    /// </summary>
    public class CAutoUpdateProvider : CServiceProvider
    {

        public CAutoUpdateProvider(CApplication app) : base(app)
        {
            base.Event.On(app, CApplication.Events.ON_INITED_CALLBACK , ()=> {

                app.Make<CAutoUpdate>().UpdateAsset();

            });
        }

        public override void Register()
        {
            application.Singleton<CAutoUpdate , CAutoUpdate>();
        }

    }

}