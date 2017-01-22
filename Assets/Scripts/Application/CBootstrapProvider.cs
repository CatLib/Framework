using UnityEngine;
using CatLib.Container;
using CatLib.Base;
using CatLib.Lua;
using CatLib.Contracts.Lua;
using CatLib.Contracts.ResourcesSystem;
using CatLib.Contracts.Event;

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
            ILua lua = application.Make<ILua>();
            if (lua is IEvent)
            {
                (lua as IEvent).Event.One(CLua.Events.ON_HOT_FIXED_COMPLETE, (sender, e) =>
                {

                    Debug.Log("hot fixed complete");

                    GameObject obj = application.Make<IResources>().Load<GameObject>("prefab/asset6/test-prefab");

                    GameObject.Instantiate(obj);

                });
            }
        }

        public override void Register()
        {
           
        }
    }
}