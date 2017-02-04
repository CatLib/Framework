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

namespace App
{

    public class CBootstrapProvider : CServiceProvider
    {

        public CBootstrapProvider(IApplication app) : base(app)
        {
        }

        public override void Init()
        {
            //todo:
            ILua lua = application.Make<ILua>();
            if (lua is IEvent)
            {
                (lua as IEvent).Event.One(CLua.Events.ON_HOT_FIXED_COMPLETE, (sender, e) =>
                {

                    IConnectorShort conn1 = FNetwork.Instance.Create<IConnectorShort>("test1");
                    IConnectorShort conn2 = FNetwork.Instance.Create<IConnectorShort>("test2");
                    IConnectorShort conn3 = FNetwork.Instance.Create<IConnectorShort>("test3");

                    FDispatcher.Instance.Event.One(conn1.GetType().ToString(), (obj1, obj2) =>
                    {
                        Debug.Log("from type one:" + (obj2 as CWebRequestEventArgs).Request.downloadHandler.text);
                    });

                    FDispatcher.Instance.Event.On(conn1.GetType().ToString(), (obj1, obj2) =>
                    {
                        Debug.Log("from type on:" + (obj2 as CWebRequestEventArgs).Request.downloadHandler.text);
                    });

                    FDispatcher.Instance.Event.One((conn1 as IGuid).TypeGuid, (obj1, obj2) =>
                    {

                        Debug.Log("from class1:" + (obj2 as CWebRequestEventArgs).Request.downloadHandler.text);
                    });
                    FDispatcher.Instance.Event.One((conn2 as IGuid).TypeGuid, (obj1, obj2) =>
                    {
                        Debug.Log("from class2:" + (obj2 as CWebRequestEventArgs).Request.downloadHandler.text);
                    });

                    conn3.SetUrl("http://127.0.0.1/testcookie.php");
                    conn3.Send(null);
                    conn3.Send(null);

                    conn1.SetUrl("http://www.52softs.com");
                    conn1.Send(new byte[] { });

                    conn2.SetUrl("http://www.baidu.com");
                    conn2.Send(new byte[] { });

                    




                    Debug.Log("hot fixed complete");

                    GameObject obj = application.Make<IResources>().Load<GameObject>("prefab/asset6/test-prefab");

                    GameObject.Instantiate(obj);

                });
            }
        }

        public override void Register()
        {
           
        }
    }
}