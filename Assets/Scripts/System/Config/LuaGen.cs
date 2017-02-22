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
                    typeof(Component),
                    typeof(LuaMonoComponent),
                    typeof(FAutoUpdate),
                    typeof(FLua),
                    typeof(FNetwork),
                    typeof(FResources),
                    typeof(API.IApplication),
                    typeof(API.Container.IBindData),
                    typeof(API.Container.IContainer),
                    typeof(API.Container.IGivenData),
                    typeof(API.Event.IDispatcher),
                    typeof(API.Event.IEvent),
                    typeof(API.Lua.ILua),
                    typeof(API.Network.IConnector),
                    typeof(API.Network.IConnectorHttp),
                    typeof(API.Network.IConnectorSocket),
                    typeof(API.Network.INetwork),
                    typeof(API.Network.ERestful),
                    typeof(API.ResourcesSystem.IResources),
                    typeof(API.UpdateSystem.IAutoUpdate),
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