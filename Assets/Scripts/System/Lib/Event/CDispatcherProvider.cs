using UnityEngine;
using System.Collections;
using CatLib.Base;
using CatLib.Container;
using CatLib.Contracts.Event;
using CatLib.Contracts.Base;

namespace CatLib.Event
{

    /// <summary>
    /// 事件调度服务
    /// </summary>
    public class CDispatcherProvider : CServiceProvider
    {

        public override void Register()
        {
            App.Bind<CEvent>().Alias<IEvent>().Alias<IEventAchieve>();
        }

    }

}