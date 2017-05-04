/*
 * This file is part of the CatLib package.
 *
 * (c) Ming ming <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using System.Text;
using CatLib.API;
using CatLib.API.MsgPack;
using CatLib.API.IO;
using UnityEngine;

namespace CatLib.Demo.MsgPack
{
    public class MsgPackDemo : ServiceProvider
    {

        public override void Init()
        {
            Env env = App[typeof(Env).ToString()] as Env;

            App.On(ApplicationEvents.ON_APPLICATION_START_COMPLETE, (sender, e) =>
                {
                    IMsgPack msgPack = App.Make<IMsgPack>();


                    MsgPackTest1Data test1 = new MsgPackTest1Data();
                    byte[] bytes = msgPack.Serializers<MsgPackTest1Data>(test1);
                    Debug.Log("bytes len:" + bytes.Length);

                    MsgPackTest1Data test1Des = msgPack.UnSerializers<MsgPackTest1Data>(bytes);
                    Debug.Log("UnSerializers:" + TinyJson.JSONWriter.ToJson(test1Des));


                    MsgPackTest2Data test2 = new MsgPackTest2Data();

                    bytes = msgPack.Serializers<MsgPackTest2Data>(test2);
                    Debug.Log("bytes len:" + bytes.Length);

                    MsgPackTest2Data test2Des = msgPack.UnSerializers<MsgPackTest2Data>(bytes);
                    Debug.Log("UnSerializers:" + TinyJson.JSONWriter.ToJson(test2Des));


                    MsgPackTest3Data test3 = new MsgPackTest3Data();

                    bytes = msgPack.Serializers<MsgPackTest3Data>(test3);
                    Debug.Log("bytes len:" + bytes.Length);
                    
                    MsgPackTest3Data test3Des = msgPack.UnSerializers<MsgPackTest3Data>(bytes);
                    Debug.Log("UnSerializers:" + TinyJson.JSONWriter.ToJson(test3Des));
                });
        }

        public override void Register() { }
    }

}
