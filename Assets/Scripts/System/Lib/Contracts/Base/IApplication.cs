using System.Collections;
using CatLib.Contracts.Container;
using CatLib.Contracts.Event;
using System;
using XLua;

namespace CatLib.Contracts.Base
{
    [LuaCallCSharp]
    public interface IApplication : IContainer , IDispatcher
    {

        IApplication Bootstrap(Type[] bootstraps);

        void Init();

        void Register(Type t);

        UnityEngine.Coroutine StartCoroutine(IEnumerator routine);

        long GetGuid();

    }

}
