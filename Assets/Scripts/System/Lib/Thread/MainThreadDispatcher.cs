using CatLib.Contracts;
using CatLib.Contracts.Thread;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CatLib.Thread
{

    /// <summary>
    /// 主线程调度器
    /// </summary>
    public class MainThreadDispatcher : Component , IUpdate , IMainThreadDispatcher
    {

        private Queue<Action> queue = new Queue<Action>();

        public void Update()
        {
            lock (queue)
            {
                while (queue.Count > 0)
                {
                    queue.Dequeue().Invoke();
                }
            }
        }

        public void Enqueue(IEnumerator action)
        {
            lock (queue)
            {
                queue.Enqueue(() => {
                    App.StartCoroutine(action);
                });
            }
        }

        public void Enqueue(Action action)
        {
            Enqueue(ActionWrapper(action));
        }

        private IEnumerator ActionWrapper(Action action)
        {
            action.Invoke();
            yield return null;
        }

    }

}