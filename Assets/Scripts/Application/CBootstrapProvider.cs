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
using CatLib.Network.UnityWebRequest;
using CatLib.Network.HttpWebRequest;

namespace App
{

    public class CBootstrapProvider : CServiceProvider
    {

        public override void Init()
        {

            Application.Event.One(CApplicationEvents.ON_APPLICATION_START_COMPLETE_CALLBACK, (sender, e) =>
            {

                IConnectorHttp conn1 = FNetwork.Instance.Create<IConnectorHttp>("test1");
                IConnectorHttp conn2 = FNetwork.Instance.Create<IConnectorHttp>("test2");
                IConnectorHttp conn3 = FNetwork.Instance.Create<IConnectorHttp>("test3");
                IConnectorHttp conn4 = FNetwork.Instance.Create<IConnectorHttp>("test4", "testcookie");

                /*
                FDispatcher.Instance.Event.One(conn1.GetType().ToString(), (obj1, obj2) =>
                {
                    Debug.Log("from type one:" + (obj2 as CWebRequestEventArgs).Request.downloadHandler.text);
                });*/

                FDispatcher.Instance.Event.On(conn4.GetType().ToString(), (obj1, obj2) =>
                {
                    Debug.Log("收到了回应");
                    Debug.Log((obj2 as CHttpRequestEventArgs).Request.Response.IsError);
                    Debug.Log((obj2 as CHttpRequestEventArgs).Request.Response.Text);
                    /*foreach(var a in (obj2 as CWebRequestEventArgs).Request.GetResponseHeaders())
                    {
                        Debug.Log(a.Key + "," + a.Value);
                    }*/
                    //Debug.Log("from type on:" +);
                });

                conn4.SetUrl("http://www.baidu.com");
                conn4.Get("");
                conn4.Get("");

                /*
                FDispatcher.Instance.Event.One((conn1 as IGuid).TypeGuid, (obj1, obj2) =>
                {

                    Debug.Log("from class1:" + (obj2 as CWebRequestEventArgs).Request.downloadHandler.text);
                });
                FDispatcher.Instance.Event.One((conn2 as IGuid).TypeGuid, (obj1, obj2) =>
                {
                    Debug.Log("from class2:" + (obj2 as CWebRequestEventArgs).Request.downloadHandler.text);
                });
                */

                /*
                conn3.SetUrl("http://127.0.0.1/testcookie.php");
                conn3.Put(string.Empty , null);*/
                //conn3.Send(null);

                /*
                conn1.SetUrl("http://www.52softs.com");
                conn1.Send(new byte[] { });

                conn2.SetUrl("http://www.baidu.com");
                conn2.Send(new byte[] { });*/






                Debug.Log("hot fixed complete");

                GameObject obj = Application.Make<IResources>().Load<GameObject>("prefab/asset6/test-prefab");

                GameObject.Instantiate(obj);

            });

        }

        public override void Register()
        {
           
        }
    }
}