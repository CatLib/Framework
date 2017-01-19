using UnityEngine;
using System.Collections;
using CatLib.UpdateSystem;
using CatLib.Container;
using CatLib.Base;

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
            application.Make<CAutoUpdate>().Event.One(CAutoUpdate.Events.ON_UPDATE_COMPLETE, (sender, e) => {

                Debug.Log("update complete");

            });
        }

        public override void Register()
        {
           
        }
    }
}