using UnityEngine;
using System.Collections;
using CatLib.Base;
using CatLib.Container;

namespace CatLib.Lua
{

    public class CLuaProvider : CServiceProvider
    {

        public CLuaProvider(CApplication app) : base(app)
        {
            app.Event.One(CApplication.Events.ON_INITED_CALLBACK, (sender, e) =>
            {

                app.Make<CLua>().LoadHotFix();

            });
        }

        public override void Register()
        {
            application.Singleton<CLua, CLua>();
        }
    }
}
