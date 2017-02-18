using UnityEngine;
using System.Text;
using CatLib;
using CatLib.Network;
using CatLib.Contracts.Network;
using CatLib.Contracts.ResourcesSystem;

public class Bootstrap : ServiceProvider
{

    public override void Init()
    {
        App.Event.One(ApplicationEvents.ON_APPLICATION_START_COMPLETE, (sender, e) =>
        {

            App.On(HttpRequestEvents.ON_MESSAGE + typeof(IConnectorHttp).ToString(), (obj1, obj2) =>
            {

                Debug.Log((obj2 as IHttpResponse).Text);
                Debug.Log((obj2 as IHttpResponse).IsError);
                Debug.Log((obj2 as IHttpResponse).Error);

            });

            App.On(TcpRequestEvents.ON_MESSAGE + typeof(IConnectorTcp).ToString(), (obj1, obj2) =>
            {

                if ((obj2 as PackageResponseEventArgs).Response.Package is string)
                {
                    Debug.Log((obj2 as PackageResponseEventArgs).Response.Package as string);
                }else
                {
                    Debug.Log(Encoding.UTF8.GetString(((obj2 as PackageResponseEventArgs).Response.Package as byte[])));
                }

            });

            App.On(TcpRequestEvents.ON_CONNECT, (obj1, obj2) =>
            {

                Debug.Log("on connect");

            });


            App.On(TcpRequestEvents.ON_ERROR, (obj1, obj2) =>
            {

                Debug.Log("on tcp error:" + (obj2 as ErrorEventArgs).Error.Message);

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
