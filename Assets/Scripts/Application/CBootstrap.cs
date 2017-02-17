using UnityEngine;
using CatLib.Container;
using CatLib.Base;
using CatLib.Contracts.ResourcesSystem;
using CatLib.Contracts.Network;
using CatLib.Network;
using System.Text;

namespace App
{

    public class CBootstrap : CServiceProvider
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

                    Debug.Log((obj2 as CPackageResponseEventArgs).Response.Package as string);

                });

                App.On(CTcpRequestEvents.ON_CONNECT, (obj1, obj2) =>
                {

                    Debug.Log("on connect");

                });


                App.On(CTcpRequestEvents.ON_ERROR, (obj1, obj2) =>
                {

                    Debug.Log("on tcp error:" + (obj2 as CErrorEventArgs).Error.Message);

                });

                /*IConnectorHttp httpConnect = FNetwork.Instance.Create<IConnectorHttp>("connector.test");
                httpConnect.Post("", "helloworld".ToByte());*/

                
                IConnectorTcp tcpConnect = FNetwork.Instance.Create<IConnectorTcp>("connector.test.tcp");
                tcpConnect.Connect();
                tcpConnect.Send("connector.test.tcp\r\n".ToByte());

                IConnectorTcp tcpConnect2 = FNetwork.Instance.Create<IConnectorTcp>("connector.test.tcp.packing.text");
                tcpConnect2.Connect();
                tcpConnect2.Send("connector.test.tcp.packing.text".ToByte());
                

                Object.Instantiate(App.Make<IResources>().Load<GameObject>("prefab/asset6/test-prefab"));

            });

        }

        public override void Register()
        {
           
        }
    }
}