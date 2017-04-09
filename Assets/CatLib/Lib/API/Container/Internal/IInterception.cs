
using System;

namespace CatLib.API.Container
{

    /// <summary>
    /// 拦截器脚本
    /// </summary>
    public interface IInterception
    {

        object Interception(IMethodInvoke methodInvoke, Func<object> next);

    }

}