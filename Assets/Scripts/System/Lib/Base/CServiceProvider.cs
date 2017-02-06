using UnityEngine;
using System.Collections;
using CatLib.Event;
using CatLib.Base;
using CatLib.Contracts.Base;

namespace CatLib.Base
{
    /// <summary>
    /// 服务提供者
    /// </summary>
    public abstract class CServiceProvider : CComponent
    {

        protected IApplication application;

        public CServiceProvider(IApplication app)
        {
            application = app;
        }

        public virtual void Init() { }

        abstract public void Register();

    }

}