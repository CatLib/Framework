using UnityEngine;
using System.Collections;
using CatLib.UpdateSystem;
using CatLib.Container;

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
            base.Event.On(application.Make<CAutoUpdate>(), CAutoUpdate.Events.ON_UPDATE_COMPLETE, () =>
            {

                Debug.Log("update complete");

            });
        }

        public override void Register()
        {
           
        }
    }
}