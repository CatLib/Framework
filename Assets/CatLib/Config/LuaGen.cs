using XLua;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CatLib.Config
{

    public class LuaGen : GenConfig
    {


        public List<Type> LuaCallCSharp
        {

            get
            {
                return new List<Type>() {

                    typeof(Input),
                    typeof(AudioSource),
                    typeof(AudioClip),
                    typeof(Application),
                    typeof(App),
                    typeof(Component),
                    typeof(MonoComponent),
                    typeof(LuaMonoComponent),
                    typeof(API.IApplication),
                    typeof(API.Container.IBindData),
                    typeof(API.Container.IContainer),
                    typeof(API.Container.IGivenData),
                    typeof(API.Event.IEvent),
                    typeof(API.Network.IConnector),
                    typeof(API.Network.IConnectorHttp),
                    typeof(API.Network.IConnectorSocket),
                    typeof(API.Network.INetworkFactory),
                    typeof(API.Network.Restfuls),
                    typeof(API.Resources.IResources),
                    typeof(API.AutoUpdate.IAutoUpdate),
                    typeof(Env),

                };
            }
        }

        public List<Type> CSharpCallLua
        {
            get
            {
                return new List<Type>() {

                    typeof(Action),
                    typeof(Action<UnityEngine.Object>),

                 };
            }
        }

        public List<List<string>> BlackList
        {
            get
            {
                return new List<List<string>>() {

                    new List<string>(){"UnityEngine.Input", "IsJoystickPreconfigured" , "System.String"},

                };
            }
        }

    }

}