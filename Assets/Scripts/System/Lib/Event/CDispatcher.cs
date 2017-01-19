using UnityEngine;
using System.Collections;
using CatLib.Container;
using CatLib.Base;

namespace CatLib.Event
{
    /// <summary>
    /// 全局事件调度器
    /// </summary>
    public class CDispatcher : CComponent
    {

        [CDependency]
        public CApplication Application { get; set; }

    }

}