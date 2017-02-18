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
                    typeof(Contracts.IApplication),
                    typeof(Contracts.Container.IBindData),
                    typeof(Contracts.Container.IContainer),
                    typeof(Contracts.Container.IGivenData),
                    typeof(Contracts.Event.IDispatcher),
                    typeof(Contracts.Event.IEvent),
                    typeof(Contracts.Lua.ILua),
                    typeof(Contracts.Network.IConnector),
                    typeof(Contracts.Network.IConnectorHttp),
                    typeof(Contracts.Network.IConnectorSocket),
                    typeof(Contracts.Network.INetwork),
                    typeof(Contracts.Network.ERestful),
                    typeof(Contracts.ResourcesSystem.IResources),
                    typeof(Contracts.UpdateSystem.IAutoUpdate),
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