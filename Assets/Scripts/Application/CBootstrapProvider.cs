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

            App.Event.One(CApplicationEvents.ON_APPLICATION_START_COMPLETE, (sender, e) =>
            {

                App.On(CHttpRequestEvents.ON_MESSAGE + typeof(IConnectorHttp).ToString(), (obj1, obj2) =>
                {

                    Debug.Log((obj2 as IHttpResponse).Text);
                    Debug.Log((obj2 as IHttpResponse).IsError);
                    Debug.Log((obj2 as IHttpResponse).Error);

                });

                App.On(CTcpRequestEvents.ON_MESSAGE + typeof(IConnectorTcp).ToString(), (obj1, obj2) =>
                {

                    Debug.Log((obj2).GetType().ToString());

                });

                App.On(CTcpRequestEvents.ON_CONNECT, (obj1, obj2) =>
                {

                    Debug.Log("on connect");

                });


                App.On(CTcpRequestEvents.ON_ERROR, (obj1, obj2) =>
                {

                    Debug.Log("on tcp error:" + (obj2 as CErrorEventArgs).Error.Message);

                });

                IConnectorHttp httpConnect = FNetwork.Instance.Create<IConnectorHttp>("test");
                httpConnect.Post("", "helloworld".ToByte());

                IConnectorTcp tcpConnect = FNetwork.Instance.Create<IConnectorTcp>("testtcp");
                tcpConnect.Connect();
                //tcpConnect.Send("hello world".ToByte());


                Object.Instantiate(App.Make<IResources>().Load<GameObject>("prefab/asset6/test-prefab"));

            });

        }

        public override void Register()
        {
           
        }
    }
}