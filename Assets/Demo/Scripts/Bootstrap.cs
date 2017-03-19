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
using CatLib.API;
using CatLib.API.TimeQueue;
using CatLib.API.INI;
using CatLib.API.IO;
using CatLib.API.Translator;
using CatLib.API.Json;
using CatLib.API.Compress;
using CatLib.API.DataTable;
using System.Collections.Generic;
using CatLib.API.Protobuf;
using CatLib.API.Csv;
using CatLib.API.CsvStore;
using System.Runtime.Serialization;
using ProtoBuf;
using CatLib.API.LocalSetting;
using CatLib.API.FilterChain;
using System.Text.RegularExpressions;
using CatLib.API.Routing;
using System;


public class EventContainerComponent : CatLib.Component
{
    public void Call(){
        Event.Trigger("test_event",this,System.EventArgs.Empty);
    }
}

public class ControllerComponent
{
    public ControllerComponent(){

        var container = new EventContainerComponent();
        container.Event.On("test_event" , (sender , e)=>{
            Debug.Log("hello");
        });
    }
}

class Test
{

    public Test() { }

    /*
    public Test(string res, IResources res2)
    {
        Debug.Log(res2);
        Debug.Log(res);

    }*/

}

public class Foos 
{ 
    public int Value;

    public object Value2;

    public List<Foosub> SubList;
}

[System.Serializable]
public class Foosub{

    public int Hello;

    public bool IsTrue;

}

public class Bootstrap : ServiceProvider
{
    public override void Init()
    {
        /*
        IEventHandler h = App.On(ApplicationEvents.ON_INITED,(sender,e)=>{

            Debug.Log("9123891237012897312");
        });

        IEventHandler aa = App.On(ApplicationEvents.ON_INITED,(sender,e)=>{

            Debug.Log("aksldjalkds9123891237012897312");
        });

        IEventHandler bb = App.On(ApplicationEvents.ON_INITED,(sender,e)=>{

            Debug.Log("ooooooooo9123891237012897312");
        });*/
        

        App.On(ApplicationEvents.ON_APPLICATION_START_COMPLETE, (sender, e) =>
        {
 
            IRouter router = App.Make<IRouter>();

            router.Reg("main/hash/{name}/{age?}", (request, response) =>
           {

               Debug.Log("hash routing");

           });


            router.Dispatch("catlib://main/hash/yubin/18");

            return;



            Debug.Log(Regex.Replace("/home/{lik{bbb}e?}/", @"\{(\w+?)\?\}", "{$1}" ));
            return;
            Uri uri = new Uri("catlib://yb199478:123456@hello/home/like/?myname=123&ss=222");

            Debug.Log("UserInfo:" + uri.UserInfo);
            Debug.Log("DnsSafeHost:" + uri.DnsSafeHost);
            Debug.Log("Fragment:" + uri.Fragment);
            Debug.Log("Authority:" + uri.Authority); 
            Debug.Log("Query:" + uri.Query);
            Debug.Log("AbsolutePath:" + uri.AbsolutePath);
            foreach (var s in uri.Segments)
            {
                Debug.Log("Segments:" + s);
            }
            Debug.Log("Scheme:" + uri.Scheme);

            return;

            IFilterChain fc = App.Make<IFilterChain>();

            IFilterChain<string> bb = fc.Create<string>();

            bb.Add((inData, chain) =>
            {
                Debug.Log("first:" + inData);
                inData = "zzzz";
                chain.Do(inData);
                Debug.Log("first:" + inData);
            });
            bb.Add((inData, chain) =>
            {
                Debug.Log("second:" + inData);
                chain.Do(inData);
                Debug.Log("second:" + inData); //string的不变性
            });

            bb.Then((inData) =>
            {
                inData = "end";
                Debug.Log("end");
            });

            bb.Do("hello");

            return;
            var abs = new Foosub() { Hello = 100 , IsTrue = true };

            ILocalSetting setting = App.Make<ILocalSetting>();
            setting.SetObject("testobj", abs);
            setting.SetInt("aa", 19);
            var abc = setting.GetObject<Foosub>("testobj");

            Debug.Log(abc.Hello);
            Debug.Log(abc.IsTrue);
            Debug.Log(setting.GetInt("aa"));
            Debug.Log(setting.GetInt("ab",100));
            return;

            ITimeQueue queue = App.Make<ITimeQueue>();

            Debug.Log(Time.frameCount);
            queue.Task(() =>
            {
                Debug.Log("time queue complete:" + Time.frameCount);
            }).LoopFrame(10).Play();

            /* 
            queue.Task(() =>
            {
                Debug.Log("time queue complete");
            }).Delay(3).Loop(5).Delay(5).Loop(()=>{

                return Random.Range(0,100) > 10;
            
            }).OnComplete(()=>{
                Debug.Log("query complete");  
            }).Play();

            FThread.Instance.Task(() =>
            {
                if(queue.Replay()){
  
                    Debug.Log("reset complete!");

                }else{

                    throw new System.Exception("faild");

                }
            }).Delay(5).Start();*/
            
            return;


            ICsvStore csvStore = App.Make<ICsvStore>();

            foreach(var v in csvStore["csv2"].Where((selector)=>
                            {
                                selector.Where("name", "=", "小兔子").OrWhere("tag", "<", "3");
                            }).Where("tag", "<", "1000").Get()){

                Debug.Log(v["name"]);

            }

            return;


            IEnv env = App.Make<IEnv>();
            IIOFactory fac = App.Make<IIOFactory>();
            IDisk disk = fac.Disk();

            IFile fff = disk.File(env.ResourcesNoBuildPath + System.IO.Path.AltDirectorySeparatorChar + "csv" + System.IO.Path.AltDirectorySeparatorChar + "csv2.csv");

            string ssss = System.Text.Encoding.UTF8.GetString(fff.Read());
            
            /* 
            string[] ss2 = ssss.Split(new string[]{ System.Environment.NewLine}, System.StringSplitOptions.RemoveEmptyEntries );
            
            foreach(var ss in ss2){
            Debug.Log(ss);
            }

            return;*/

            ICsvParser csvParser = App.Make<ICsvParser>();

            string[][] parser = csvParser.Parser(ssss);

            /* 
            foreach(var v in parser){

                Debug.Log(v[0] + "|" + v[1] + "|" + v[2]);

            }*/
            
            

            IDataTableFactory dataTable = App.Make<IDataTableFactory>();
            IDataTable table = dataTable.Make(parser);

            foreach(var v in table.Where((selector)=>
            {

                selector.Where("name", "=", "小兔子").OrWhere("tag", "<", "3");

            }).Where("tag", "<", "1000").Get()){

                Debug.Log(v["name"]);

            }

            return;


            foreach(string[] s in parser)
            {

                Debug.Log(s[0] + "|" + s[1] + "|" + s[2]);

            }

            return;

            IProtobuf protobuf = App.Make<IProtobuf>();

            var person = new TestProto.Person
            {
                Id = 12345,
                Name = "Fred",
                Address = new TestProto.Address
                {
                    Line1 = "Flat 1",
                    Line2 = "The Meadows"
                }
            };

            byte[] data = protobuf.Serializers(person);

            Debug.Log(data.Length);

            var p = protobuf.UnSerializers<TestProto.Person>(data);

            Debug.Log(p.Name);

            IFile file = disk.File("hello.gz");
            ICompress comp = App.Make<ICompress>();
            byte[] byt = comp.Compress("helloworldhelloworldhelloworldhelloworldhelloworldhelloworldhelloworldhelloworldhelloworld".ToByte());

            Debug.Log("UnCompress: "+ "helloworldhelloworldhelloworldhelloworldhelloworldhelloworldhelloworldhelloworldhelloworld".ToByte().Length);
            Debug.Log("Compress: "+ byt.Length);
            //file.Create(byt);

            byt = comp.UnCompress(byt);
            Debug.Log(System.Text.Encoding.UTF8.GetString(byt));

            //byte[] debyt = comp.Expand(byt);

            //Debug.Log(System.Text.Encoding.UTF8.GetString(debyt));

            
            IJson json = App.Make<IJson>();

            Foos ff = new Foos();
            ff.Value = 100;
            ff.Value2 = "123";
            ff.SubList = new List<Foosub>();
            ff.SubList.Add(new Foosub(){ Hello = 10 , IsTrue = true});
            ff.SubList.Add(new Foosub(){ Hello = 20 , IsTrue = true});
            ff.SubList.Add(new Foosub(){ Hello = 30 , IsTrue = false});

            Debug.Log(json.Encode(ff));

            Foos f = json.Decode<Foos>(json.Encode(ff));
            Debug.Log(f.Value2);
            foreach(Foosub sb in f.SubList){

                Debug.Log(sb.Hello);

            }

            //Debug.Log(f.Value);

            
            ITranslator tran = App.Make<ITranslator>();
            Debug.Log(tran.Trans("test.messages3"));
            Debug.Log(tran.Trans("test.messages3" , "age:18" , "name" , "anny"));
            Debug.Log(tran.Trans("test.message" , "name:anny"));
            Debug.Log(tran.TransChoice("test.messages" , 0 , "name" , "喵喵"));
            Debug.Log(tran.TransChoice("test.messages" , 12 , "name" , "miaomiao"));
            Debug.Log(tran.TransChoice("test.messages" , 20 , "name" , "miaomiao"));
            tran.SetLocale("en");
            Debug.Log(tran.Trans("test.message" , "name" , "喵喵"));
            

            //IEnv env = App.Make<IEnv>();
            //IDisk disk = App.Make<IIOFactory>().Disk();
            //IINIResult result = App.Make<IINILoader>().Load(disk.File(env.ResourcesNoBuildPath + System.IO.Path.AltDirectorySeparatorChar + "/lang/cn/test.ini"));
            //result.Set("helloworld", "mynameisyb", "yb");
            //result.Remove("myname");
            //result.Save();


            IResources res = App.Make<IResources>();
            /*res.LoadAsync("prefab/asset6/test-prefab",(a)=>
            {
                a.Instantiate();
            });
            res.LoadAsync("prefab/asset6/test-prefab2", (a) =>
            {
                a.Instantiate();
            });*/
            //var b = res.Load<Object>("prefab/asset6/test-prefab");
            res.LoadAsync("prefab/asset6/test-prefab", (aa) =>
            {
                var dd = aa.Instantiate();

                App.Make<ITimeQueue>().Task(() =>
                {

                    Debug.Log("now destroy 1 prefab");
                    GameObject.Destroy(dd);

                }).Delay(20).Play();
            });
            //var a = res.Load("prefab/asset6/test-prefab2");
            //GameObject obj = a.Instantiate();
            //GameObject.Instantiate(obj); //绕过控制克隆
            
            /*App.Make<ITimeQueue>().Task(() =>
            {

                Debug.Log("now destroy 1 prefab");
                GameObject.Destroy(obj);

            }).Delay(10).Play();*/
            
            /*
            IResources res = App.Make<IResources>();
            res.LoadAsync<GameObject>("prefab/asset6/test-prefab", (obj) =>
            {
                Object.Instantiate(obj);
            });

            //h.Cancel();

            //App.Event.Trigger(ApplicationEvents.ON_INITED);
            
            //Debug.Log(App.Make(typeof(Test).ToString(),"123"));

            //IHash hash = App.Make<IHash>();
            //Debug.Log(hash.Bcrypt("helloworld"));

            //ICrypt secret = App.Make<ICrypt>();
            //string code = secret.Encrypt("helloworld");
            //Debug.Log(code);

            //Debug.Log(secret.Decrypt(code));

            /*FThread.Instance.Task(() =>
            {
                Debug.Log("pppppppppppppppppppp");
                int i = 0;
                i++;
                return i;
            }).Delay(5).Start().Cancel();
            */
            //Debug.Log(hash.BcryptVerify("helloworld", "$2a$10$Y8BxbHFgGArGVHIucx8i7u7t5ByLlSdWgWcQc187hqFfSiKFJfz3C"));
            //Debug.Log(hash.BcryptVerify("helloworld", "$2a$15$td2ASPNq.8BXbpa6yUU0c.pQpfYLxtcbXviM8fZXw4v8FDeO3hCoC"));


            //IAssetBundle bundle = App.Make<IAssetBundle>();
            //Object.Instantiate(res.Load<GameObject>("prefab/asset6/test-prefab.prefab"));

            //Object[] p = res.LoadAll("prefab/asset6");
            //IResources res = App.Make<IResources>();
            /*res.LoadAsync<GameObject>("prefab/asset6/test-prefab", (obj) =>
             {
                 Object.Instantiate(obj);
             });*/
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
            ITimeQueue timeQueue = App.Make<ITimeQueue>();
            
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

            timeQueue.Play(); */
            /* 

                        FThread.Instance.Task(() =>
                        {
                            Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                            //Debug.Log(App.Time.Time);
                            timeQueue.Replay();

                        }).Delay(9).Start();*/

            
            App.On(HttpRequestEvents.ON_MESSAGE + typeof(IConnectorHttp).ToString(), (obj1, obj2) =>
            {

                Debug.Log((obj2 as IHttpResponse).Text);
                Debug.Log((obj2 as IHttpResponse).IsError);
                Debug.Log((obj2 as IHttpResponse).Error);

            });

            IConnectorHttp http = FNetwork.Instance.Create<IConnectorHttp>("http");
            //http.SetConfig(new System.Collections.Hashtable() { { "host", "http://www.qidian.com/" } });
            http.Get(string.Empty);

            
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
                Debug.Log((e1 as PackageResponseEventArgs));
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
            /* 
            IConnectorUdp udpConnect = FNetwork.Instance.Create<IConnectorUdp>("udp.bind.host.text");
            udpConnect.Connect();
            udpConnect.Send("hello this is udp msg with [text]".ToByte());
            
            
            IConnectorUdp udpConnectFrame = FNetwork.Instance.Create<IConnectorUdp>("udp.bind.host.frame");
            udpConnectFrame.Connect();
            udpConnectFrame.Send("hello this is udp msg with [frame]".ToByte());*/
           
            
            IConnectorUdp udpConnect2 = FNetwork.Instance.Create<IConnectorUdp>("udp.unbind.host.frame");
            udpConnect2.Connect();
            udpConnect2.Send("hello world(client udp)".ToByte() , "pvp.gift", 3301);


        });

    }

    public override void Register()
    {
           
    }
}
