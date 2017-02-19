using System.Collections;
using CatLib.Contracts.Container;
using CatLib.Contracts.Event;
using System;
using CatLib.Contracts.Time;

namespace CatLib.Contracts
{

    public interface IApplication : IContainer , IDispatcher
    {

        IApplication Bootstrap(Type[] bootstraps);

        void Init();

        void Register(Type t);

        UnityEngine.Coroutine StartCoroutine(IEnumerator routine);

        long GetGuid();

        bool IsMainThread { get; }

        void MainThread(IEnumerator action);

        void MainThread(Action action);

        ITime Time { get; }

    }

}
