using UnityEngine;
using System.Collections;
using CatLib.Base;
using CatLib.Container;
using CatLib.Contracts.Event;

namespace CatLib.Event
{

    /// <summary>
    /// 事件调度服务
    /// </summary>
    public class CDispatcherProvider : CServiceProvider
    {

        public CDispatcherProvider(CApplication app) : base(app)
        {
        }

        public override void Register()
        {
            application.Singleton<CDispatcher>().Alias<IDispatcher>();
        }

    }

}