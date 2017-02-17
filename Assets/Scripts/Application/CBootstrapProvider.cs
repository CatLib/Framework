using UnityEngine;
using CatLib.Container;
using CatLib.Base;
using CatLib.Contracts.ResourcesSystem;
using CatLib.Contracts.Network;
using CatLib.Network;
using System.Text;

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
                tcpConnect.Send("hello world\r\n".ToByte());


                CatLib.Support.CBuffer buff = new CatLib.Support.CBuffer();

                buff.Push("helloworld\r\nhelloworld".ToByte());
                int num = buff.IndexOf("\r\n".ToByte());

                buff.Unshift("\r\n".ToByte());
                var data = buff.Shift(2);
                Debug.Log(data[0] + "," + data[1]);
                if(Encoding.UTF8.GetString(data) == "\r\n"){
                    
                    Debug.Log("123");
                }

                buff.Push("999".ToByte());
                Debug.Log(Encoding.UTF8.GetString(buff.Pop(3)));

                Debug.Log(Encoding.UTF8.GetString(buff.Shift(buff.Length)));
                Debug.Log(buff.Length);

                UnityEngine.Object.Instantiate(App.Make<IResources>().Load<GameObject>("prefab/asset6/test-prefab"));

            });

        }

        public override void Register()
        {
           
        }
    }
}