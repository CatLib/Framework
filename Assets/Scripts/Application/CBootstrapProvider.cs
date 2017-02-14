using UnityEngine;
using CatLib.Container;
using CatLib.Base;
using CatLib.Lua;
using CatLib.Contracts.Lua;
using CatLib.Contracts.ResourcesSystem;
using CatLib.Contracts.Event;
using CatLib.Contracts.Network;
using CatLib.Contracts.Base;
using CatLib.Network;
using System.Collections.Generic;

namespace App
{

    public class CBootstrapProvider : CServiceProvider
    {

        public override void Init()
        {

            Application.Event.One(CApplicationEvents.ON_APPLICATION_START_COMPLETE_CALLBACK, (sender, e) =>
            {

                FDispatcher.Instance.Event.On(typeof(IConnectorHttp).ToString(), (obj1, obj2) =>
                {

                    Debug.Log((obj2 as IHttpResponse).Text);
                    Debug.Log((obj2 as IHttpResponse).IsError);
                    Debug.Log((obj2 as IHttpResponse).Error);

                });
                
                FDispatcher.Instance.Event.On(typeof(IConnectorTcp).ToString(), (obj1, obj2) =>
                {

                    Debug.Log((obj2).GetType().ToString());

                });

                

                IConnectorHttp httpConnect = FNetwork.Instance.Create<IConnectorHttp>("test");
                httpConnect.Post("", "helloworld".ToByte());

                //IConnectorTcp tcpConnect = FNetwork.Instance.Create<IConnectorTcp>("testtcp");

                //tcpConnect.Send("hello world".ToByte());


                Object.Instantiate(Application.Make<IResources>().Load<GameObject>("prefab/asset6/test-prefab"));

            });

        }

        public override void Register()
        {
           
        }
    }
}