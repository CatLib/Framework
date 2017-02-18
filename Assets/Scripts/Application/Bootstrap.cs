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

            App.On(SocketRequestEvents.ON_MESSAGE + typeof(IConnectorSocket).ToString(), (obj1, obj2) =>
            {

                if ((obj2 as PackageResponseEventArgs).Response.Package is string)
                {
                    Debug.Log((obj2 as PackageResponseEventArgs).Response.Package as string);
                }else
                {
                    Debug.Log(Encoding.UTF8.GetString(((obj2 as PackageResponseEventArgs).Response.Package as byte[])));
                }

            });

            App.On(SocketRequestEvents.ON_CONNECT, (obj1, obj2) =>
            {

                Debug.Log("on connect");

            });


            App.On(SocketRequestEvents.ON_ERROR, (obj1, obj2) =>
            {

                Debug.Log("on tcp error:" + (obj2 as ErrorEventArgs).Error.Message);

            });

            //链接配置见 NetworkConfig 配置文件

            IConnectorTcp tcpConnect = FNetwork.Instance.Create<IConnectorTcp>("tcp.text");
            tcpConnect.Connect();
            tcpConnect.Send("hello this is tcp msg with [text]".ToByte());

            IConnectorTcp tcpConnect2 = FNetwork.Instance.Create<IConnectorTcp>("tcp.frame");
            tcpConnect2.Connect();
            tcpConnect2.Send("hello this is tcp msg with [frame]".ToByte());
            
            IConnectorUdp udpConnect = FNetwork.Instance.Create<IConnectorUdp>("udp.bind.host.text");
            udpConnect.Connect();
            udpConnect.Send("hello this is udp msg with [text]".ToByte());

            IConnectorUdp udpConnectFrame = FNetwork.Instance.Create<IConnectorUdp>("udp.bind.host.frame");
            udpConnectFrame.Connect();
            udpConnectFrame.Send("hello this is udp msg with [frame]".ToByte());

            IConnectorUdp udpConnect2 = FNetwork.Instance.Create<IConnectorUdp>("udp.unbind.host.frame");
            udpConnect2.Connect();
            udpConnect2.Send("hello world(client udp)".ToByte() , "pvp.gift", 3301);


            Object.Instantiate(App.Make<IResources>().Load<GameObject>("prefab/asset6/test-prefab"));

        });

    }

    public override void Register()
    {
           
    }
}
