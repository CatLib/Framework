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

                IConnectorHttp conn1 = FNetwork.Instance.Create<IConnectorHttp>("test1");
                IConnectorHttp conn2 = FNetwork.Instance.Create<IConnectorHttp>("test2");
                IConnectorHttp conn3 = FNetwork.Instance.Create<IConnectorHttp>("test3");
                IConnectorHttp conn4 = FNetwork.Instance.Create<IConnectorHttp>("test4", "testcookie");

                /*
                FDispatcher.Instance.Event.One(conn1.GetType().ToString(), (obj1, obj2) =>
                {
                    Debug.Log("from type one:" + (obj2 as CWebRequestEventArgs).Request.downloadHandler.text);
                });*/

                FDispatcher.Instance.Event.On(typeof(IConnectorHttp).ToString(), (obj1, obj2) =>
                {

                    Debug.Log((obj2 as IHttpResponse).Text);
                    Debug.Log((obj2 as IHttpResponse).IsError);
                    Debug.Log((obj2 as IHttpResponse).Error);
                    /*
                    if ((obj2 as IHttpResponse).Restful != ERestful.DELETE)
                    {
                        Debug.Log((obj2 as IHttpResponse).Text);
                    }else
                    {
                        if((obj2 as IHttpResponse).IsError)
                        {
                            Debug.Log("delete faild");
                        }else
                        {
                            Debug.Log("delete success");
                        }
                    }
                    */
                });

                //conn4.SetUrl("http://www.baidu.com");
                conn4.SetUrl("http://127.0.0.1/testcookie.php");
                WWWForm form = new WWWForm();
                form.AddField("data", "中文你好");
                form.AddField("data-en", "en hello");

                conn4.AppendHeader("myheader" , "hello world");

                conn4.Put("", form);
                conn4.Post("", form);
                //conn4.Delete("");
                //conn4.Head("");

                /*
                conn4.Restful(ERestful.POST, string.Empty, form);
                conn4.Restful(ERestful.PATCH, string.Empty, form);
                conn4.Restful(ERestful.COPY, string.Empty , form);
                conn4.Restful(ERestful.LINK, string.Empty, form);
                conn4.Restful(ERestful.UNLINK, string.Empty, form);
                conn4.Restful(ERestful.PURGE, string.Empty, form);
                conn4.Restful(ERestful.LOCK, string.Empty, form);
                conn4.Restful(ERestful.UNLOCK, string.Empty, form);
                conn4.Restful(ERestful.PROFFIND, string.Empty, form);
                conn4.Restful(ERestful.VIEW, string.Empty, form);*/

                
                conn3.SetUrl("http://127.0.0.11/testcookie.php");
                conn3.AppendHeader("myheader", "conn3");
                //conn3.Put("", form);
                conn3.Post("", form);
                //conn3.Delete("");
                
                //conn3.Head("");
                /*conn3.Restful(ERestful.POST, string.Empty, form);
                conn3.Restful(ERestful.PATCH, string.Empty, form);
                conn3.Restful(ERestful.COPY, string.Empty, form);
                conn3.Restful(ERestful.LINK, string.Empty, form);
                conn3.Restful(ERestful.UNLINK, string.Empty, form);
                conn3.Restful(ERestful.PURGE, string.Empty, form);
                conn3.Restful(ERestful.LOCK, string.Empty, form);
                conn3.Restful(ERestful.UNLOCK, string.Empty, form);
                conn3.Restful(ERestful.PROFFIND, string.Empty, form);
                conn3.Restful(ERestful.VIEW, string.Empty, form);*/


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