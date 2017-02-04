using UnityEngine;
using System.Collections;
using CatLib.Contracts.Container;
using CatLib.Contracts.Event;
using System;

namespace CatLib.Contracts.Base
{

    public interface IApplication : IContainer , IEvent
    {

        IApplication Bootstrap(Type[] bootstraps);

        void Init();

        void Register(Type t);

        UnityEngine.Coroutine StartCoroutine(IEnumerator routine);

        long GetGuid();

    }

}
