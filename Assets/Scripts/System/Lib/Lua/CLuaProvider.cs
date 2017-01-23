using UnityEngine;
using System.Collections;
using CatLib.Base;
using CatLib.Container;
using CatLib.Contracts.Lua;

namespace CatLib.Lua
{

    public class CLuaProvider : CServiceProvider
    {

        public CLuaProvider(CApplication app) : base(app)
        {
            app.Event.One(CApplication.Events.ON_INITED_CALLBACK, (sender, e) =>
            {

                (app.Make<ILua>() as CLua).LoadHotFix();

            });
        }

        public override void Register()
        {
            application.Singleton<CLua>().Alias<ILua>();
        }
    }
}
