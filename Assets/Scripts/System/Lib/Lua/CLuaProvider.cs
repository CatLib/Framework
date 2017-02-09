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
        public override void Init()
        {
            IAutoUpdate autoUpdata = Application.Make<IAutoUpdate>();
            if (autoUpdata is IEvent)
            {
                (autoUpdata as IEvent).Event.One(CAutoUpdateEvents.ON_UPDATE_COMPLETE, (sender, e) =>
                {
                    (Application.Make<ILua>() as CLua).LoadHotFix();
                });
            }
        }


        public override void Register()
        {
            Application.Singleton<CLua>().Alias<ILua>();
        }
    }
}
