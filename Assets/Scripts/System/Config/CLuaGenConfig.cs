using UnityEngine;
using System.Collections;
using CatLib.Lua;
using System;
using System.Collections.Generic;
using XLua;
using CatLib.Container;

namespace CatLib.Config
{

    public class CLuaConfig : GenConfig
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
                    typeof(Base.CApp),
                    typeof(Base.CComponent),
                    typeof(Base.CMonoComponent),
                    typeof(Base.CComponent),
                    typeof(Base.CLuaMonoComponent),
                    typeof(Base.FAutoUpdate),
                    typeof(Base.FLua),
                    typeof(Base.FNetwork),
                    typeof(Base.FResources),
                    typeof(Contracts.Base.IApplication),
                    typeof(Contracts.Container.IBindData),
                    typeof(Contracts.Container.IContainer),
                    typeof(Contracts.Container.ITmpData),
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
                    typeof(Support.CEnv),

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