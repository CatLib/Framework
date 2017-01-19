using UnityEngine;
using System.Collections;
using CatLib.Event;
using CatLib.Base;

namespace CatLib.Base
{

    /// <summary>
    /// 服务提供者
    /// </summary>
    public abstract class CServiceProvider : CComponent
    {

        protected CApplication application;

        public CServiceProvider(CApplication app)
        {
            application = app;
        }

        public virtual void Init() { }

        abstract public void Register();

    }

}