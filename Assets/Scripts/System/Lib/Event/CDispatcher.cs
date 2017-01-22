using UnityEngine;
using System.Collections;
using CatLib.Container;
using CatLib.Base;
using CatLib.Contracts.Event;

namespace CatLib.Event
{
    /// <summary>
    /// 全局事件调度器
    /// </summary>
    public class CDispatcher : CComponent , IDispatcher
    {

        [CDependency]
        public CApplication Application { get; set; }

    }

}