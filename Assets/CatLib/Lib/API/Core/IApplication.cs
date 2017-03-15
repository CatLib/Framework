using System.Collections;
using CatLib.API.Container;
using CatLib.API.Event;
using System;
using CatLib.API.Time;

namespace CatLib.API
{

    public interface IApplication : IContainer, IEvent, IEventAchieve
    {

        IApplication Bootstrap(Type[] bootstraps);

        void Init();

        void Register(Type t);

        UnityEngine.Coroutine StartCoroutine(IEnumerator routine);

        long GetGuid();

        bool IsMainThread { get; }

        void MainThread(IEnumerator action);

        void MainThread(Action action);

        IGlobalEvent Trigger(object score);

        ITime Time { get; }

    }

}
