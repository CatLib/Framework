
using System;
using CatLib.API.Flux;

namespace CatLib.Flux
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
        /// 增加监听者
        /// </summary>
        /// <param name="action"></param>
        void AddListener(Action<IAction> action);

        /// <summary>
        /// 减少监听者
        /// </summary>
        /// <param name="action"></param>
        void RemoveListener(Action<IAction> action);

        /// <summary>
        /// 存储是否是被修改的
        /// </summary>
        bool IsChanged { get; }

        /// <summary>
        /// 存储归属的调度器
        /// </summary>
        IFluxDispatcher Dispatcher { get; }

        /// <summary>
        /// 调度器中该存储注册的token
        /// </summary>
        string DispatchToken { get; }

        /// <summary>
        /// 是否被释放的
        /// </summary>
        bool IsDestroy { get; }

        /// <summary>
        /// 释放存储
        /// </summary>
        void Destroy();
    }
}