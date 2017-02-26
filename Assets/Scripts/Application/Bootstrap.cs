using UnityEngine;
using System.Text;
using CatLib;
using CatLib.Network;
using CatLib.API.Network;
using CatLib.API.Resources;
using System.Threading;
using CatLib.API.Event;
using CatLib.API.Time;
using CatLib.API.Hash;
using CatLib.API.Crypt;

public class Bootstrap : ServiceProvider
{

    public override void Init()
    {
        App.Event.One(ApplicationEvents.ON_APPLICATION_START_COMPLETE, (sender, e) =>
        {


            IHash hash = App.Make<IHash>();
            //Debug.Log(hash.Bcrypt("helloworld"));

            ICrypt secret = App.Make<ICrypt>();
            string code = secret.Encrypt("this is need encrypt string");
            Debug.Log(code);

            Debug.Log(secret.Decrypt(code));


            //Debug.Log(hash.BcryptVerify("helloworld", "$2a$10$Y8BxbHFgGArGVHIucx8i7u7t5ByLlSdWgWcQc187hqFfSiKFJfz3C"));
            //Debug.Log(hash.BcryptVerify("helloworld", "$2a$15$td2ASPNq.8BXbpa6yUU0c.pQpfYLxtcbXviM8fZXw4v8FDeO3hCoC"));

            IResources res = App.Make<IResources>();
            IAssetBundle bundle = App.Make<IAssetBundle>();
            //Object.Instantiate(res.Load<GameObject>("prefab/asset6/test-prefab.prefab"));

            //Object[] p = res.LoadAll("prefab/asset6");

            res.LoadAsync<GameObject>("prefab/asset6/test-prefab", (obj) =>
             {
                 Object.Instantiate(obj);
             });
            //res.UnloadAll();
            //Object.Instantiate(res.Load<GameObject>("prefab/asset6/test-prefab.prefab"));

            /*
            Thread subThread = new Thread(new ThreadStart(() => {

                App.MainThread(() => { new GameObject(); });

            }));

            FThread.Instance.Task(() =>
            {
                int i = 0;
                i++;
                return i;
            }).Delay(5).OnComplete((obj) => Debug.Log("sub thread complete:" + obj)).Start();

            subThread.Start();

            */


            /*
            ITimeQueue timeQueue = App.Time.CreateQueue();
            
            ITimeTaskHandler h = timeQueue.Task(() =>
            {
                Debug.Log("this is in task");
            }).Delay(3).Loop(3).Push();

            
            timeQueue.Task(() =>
            {
                Debug.Log("2222222");
            }).Delay(1).Loop(3).OnComplete(()=> { h.Cancel(); Debug.Log("2 complete"); }).Push();
            
            timeQueue.Task(() =>
            {
                Debug.Log("rand!");
            }).Loop(() => { return Random.Range(0,100) > 10; }).Push();

            timeQueue.OnComplete(() =>
            {
                Debug.Log("queueComplete");
            });

            timeQueue.Play();

            
            FThread.Instance.Task(() =>
            {
                Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                //Debug.Log(App.Time.Time);
                timeQueue.Replay();

            }).Delay(9).Start();*/



            /*

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

            (tcpConnect as IEvent).Event.One(SocketRequestEvents.ON_MESSAGE, (s1, e1) =>
            {
                if ((e1 as PackageResponseEventArgs).Response.Package is string)
                {
                    Debug.Log((e1 as PackageResponseEventArgs).Response.Package as string);
                }
                else
                {
                    Debug.Log(Encoding.UTF8.GetString(((e1 as PackageResponseEventArgs).Response.Package as byte[])));
                }
            });

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
            */

        });

    }

    public override void Register()
    {
           
    }
}
