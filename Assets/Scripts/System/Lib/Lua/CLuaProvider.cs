using UnityEngine;
using System.Collections;
using CatLib.Base;
using CatLib.Container;
using CatLib.Contracts.Lua;
using CatLib.Contracts.UpdateSystem;
using CatLib.Contracts.Event;
using CatLib.UpdateSystem;
using CatLib.Contracts.Base;

namespace CatLib.Lua
{

    public class CLuaProvider : CServiceProvider
    {

        public CLuaProvider(IApplication app) : base(app)
        {
        }

        public override void Init()
        {
            IAutoUpdate autoUpdata = application.Make<IAutoUpdate>();
            if (autoUpdata is IEvent)
            {
                (autoUpdata as IEvent).Event.One(CAutoUpdate.Events.ON_UPDATE_COMPLETE, (sender, e) =>
                {
                    (application.Make<ILua>() as CLua).LoadHotFix();
                });
            }
        }


        public override void Register()
        {
            application.Singleton<CLua>().Alias<ILua>();
        }
    }
}
