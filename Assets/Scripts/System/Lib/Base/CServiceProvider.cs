using CatLib.Contracts.Base;
using CatLib.Container;

namespace CatLib.Base
{
    /// <summary>
    /// 服务提供者
    /// </summary>
    public abstract class CServiceProvider : CComponent
    {

        public virtual void Init() { }

        abstract public void Register();

    }

}