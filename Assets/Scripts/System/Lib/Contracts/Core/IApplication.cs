using System.Collections;
using CatLib.Contracts.Container;
using CatLib.Contracts.Event;
using System;

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

    }

}
