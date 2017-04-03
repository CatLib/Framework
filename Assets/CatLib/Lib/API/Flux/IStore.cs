
using System;

namespace CatLib.API.Flux
{

    /// <summary>
    /// 存储
    /// </summary>
    public interface IStore
    {

        /// <summary>
        /// 存储的名字
        /// </summary>
        string Name { get; }
    
        /// <summary>
        /// 增加监听
        /// </summary>
        /// <param name="action"></param>
        void AddListener(Action<INotification> action);

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <param name="action"></param>
        void RemoveListener(Action<INotification> action);

        /// <summary>
        /// 调度器
        /// </summary>
        IFluxDispatcher Dispatcher { get; }

        /// <summary>
        /// 调度token
        /// </summary>
        string DispatchToken { get; }

    }

}