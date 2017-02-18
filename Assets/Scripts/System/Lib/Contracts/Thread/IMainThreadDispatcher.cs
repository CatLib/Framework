
using System;
using System.Collections;

namespace CatLib.Contracts.Thread
{

    public interface IMainThreadDispatcher
    {

        /// <summary>
        /// 加入调度队列
        /// </summary>
        /// <param name="action"></param>
        void Enqueue(IEnumerator action);

        /// <summary>
        /// 加入调度队列
        /// </summary>
        /// <param name="action"></param>
        void Enqueue(Action action);

    }

}