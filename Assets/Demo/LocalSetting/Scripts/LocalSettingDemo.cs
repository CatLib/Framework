/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using UnityEngine;
using CatLib.API;
using CatLib.API.LocalSetting;

namespace CatLib.Demo.LocalSetting
{

    [System.Serializable]
    public class SettingObject
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class LocalSettingDemo : ServiceProvider
    {

        public override void Init()
        {
            App.On(ApplicationEvents.ON_APPLICATION_START_COMPLETE, (sender, e) =>
            {

                ILocalSetting localSetting = App.Make<ILocalSetting>();


                localSetting.SetObject("demo.object", new SettingObject() { Name = "anny" , Age = 18 });
                localSetting.SetInt("demo.int", 18);


                var obj = localSetting.GetObject<SettingObject>("demo.object");

                Debug.Log("hello my name is : " + obj.Name + " , age is " + obj.Age);
                Debug.Log("save int is:" + localSetting.GetInt("demo.int"));
                Debug.Log("empty is:" + localSetting.GetInt("demo.int.empty" , 20));

            });
        }

        public override void Register() { }
    }
}