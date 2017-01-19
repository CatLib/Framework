using UnityEngine;
using CatLib.Container;
using CatLib.Base;

namespace CatLib.UpdateSystem
{
    /// <summary>
    /// 自动更新服务提供商
    /// </summary>
    public class CAutoUpdateProvider : CServiceProvider
    {

        public CAutoUpdateProvider(CApplication app) : base(app)
        {
            app.Event.One(CApplication.Events.ON_INITED_CALLBACK, (sender, e) =>
            {

                app.Make<CAutoUpdate>().UpdateAsset();

            });
        }

        public override void Register()
        {
            application.Singleton<CAutoUpdate , CAutoUpdate>();
        }

    }

}