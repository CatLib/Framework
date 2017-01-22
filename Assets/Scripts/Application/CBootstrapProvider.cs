using UnityEngine;
using System.Collections;
using CatLib.UpdateSystem;
using CatLib.Container;
using CatLib.Base;
using CatLib.ResourcesSystem;
using CatLib.Lua;
using CatLib.Contracts.Lua;
using CatLib.Contracts.ResourcesSystem;

namespace App
{

    public class CBootstrapProvider : CServiceProvider
    {

        public CBootstrapProvider(CApplication app) : base(app)
        {
        }

        public override void Init()
        {
            //todo:
            (application.Make<ILua>() as CLua).Event.One(CLua.Events.ON_HOT_FIXED_COMPLETE, (sender, e) => {

                Debug.Log("hot fixed complete");

                GameObject obj = application.Make<IResources>().Load<GameObject>("prefab/asset6/test-prefab");

                GameObject.Instantiate(obj);

            });
        }

        public override void Register()
        {
           
        }
    }
}