using UnityEngine;
using System.Collections;
using CatLib.Container;

namespace CatLib.Event
{
    /// <summary>
    /// 全局事件调度器
    /// </summary>
    public class CDispatcher : CEvent , IEvent
    {

        [CDependency]
        public CApplication Application { get; set; }

        /// <summary>
        /// 事件
        /// </summary>
        public CEvent Event { get { return this; } }

    }

}