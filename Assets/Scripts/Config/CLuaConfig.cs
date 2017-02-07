using UnityEngine;
using System.Collections;
using CatLib.Lua;
using System;
using System.Collections.Generic;
using XLua;

public class CLuaConfig : CConfig , GenConfig
{

    /// <summary>
    /// 类
    /// </summary>
    public override Type Class
    {
        get
        {
            return typeof(CLua);
        }
    }

    public List<Type> LuaCallCSharp { 
        
        get{    
            return new List<Type>() {

                typeof(UnityEngine.Application),
                typeof(CapLib.Base.CApp),
                typeof(CatLib.Base.CComponent),
                typeof(CatLib.Base.CMonoComponent),
                typeof(CatLib.Base.CComponent),
                typeof(CatLib.Base.CLuaMonoComponent),
                typeof(CatLib.Base.FAutoUpdate),
                typeof(CatLib.Base.FDispatcher),
                typeof(CatLib.Base.FLua),
                typeof(CatLib.Base.FNetwork),
                typeof(CatLib.Base.FResources),
                typeof(CatLib.Contracts.Base.IApplication),
                typeof(CatLib.Contracts.Container.IBindData),
                typeof(CatLib.Contracts.Container.IContainer),
                typeof(CatLib.Contracts.Container.ITmpData),
                typeof(CatLib.Contracts.Event.IDispatcher),
                typeof(CatLib.Contracts.Event.IEvent),
                typeof(CatLib.Contracts.Lua.ILua),
                typeof(CatLib.Contracts.Network.IConnector),
                typeof(CatLib.Contracts.Network.IConnectorHttp),
                typeof(CatLib.Contracts.Network.IConnectorSocket),
                typeof(CatLib.Contracts.Network.INetwork),
                typeof(CatLib.Contracts.ResourcesSystem.IResources),
                typeof(CatLib.Contracts.UpdateSystem.IAutoUpdate),
                typeof(CatLib.Support.CInput),
                typeof(CatLib.Support.CEnv),

            };
        } 
    }

    public List<Type> CSharpCallLua { get{  return new List<Type>() {

                typeof(System.Action),
                typeof(System.Action<UnityEngine.Object>),

             }; 
        } 
    }

    public  List<List<string>> BlackList { get{  return new List<List<string>>() { }; } }

    /// <summary>
    /// 配置
    /// </summary>
    protected override object[] Field
    {
        get
        {
            return new object[]
            {
                //热补丁文件路径
                "lua.hotfix" , new string[] { "scripts/hotfix" }
            };
        }
    }
}
