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

        public override EProviderProcess ProviderProcess
        {
            get
            {
                return EProviderProcess.CODE_AUTO_LOAD;
            }
        }

        public override IEnumerator OnProviderProcess()
        {
            yield return (Application.Make<ILua>() as CLua).LoadHotFix();
        }


        public override void Register()
        {
            Application.Singleton<CLua>().Alias<ILua>();
        }
    }
}
