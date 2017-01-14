/*
 * 作者：喵喵
 * QQ：917684105
 */
using UnityEngine;
using System.Collections;

namespace CatLib.Event
{
    /// <summary>事件机制接口</summary>
    public interface IEvent
    {

        /// <summary>事件系统</summary>
        CEvent Event { get; }

    }
}
