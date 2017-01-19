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
        }

        public override void Register()
        {
            application.Singleton<ILua, CLua>();
        }
    }
}
