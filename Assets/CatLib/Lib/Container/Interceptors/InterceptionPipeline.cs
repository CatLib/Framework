
using System;
using CatLib.API.Container;
using System.Collections.Generic;

namespace CatLib.Container
{

    /// <summary>
    /// 拦截器管道
    /// </summary>
    public class InterceptionPipeline
    {

        public InterceptionPipeline Then(Func<object> action)
        {
            return this;
        }

        public object Do()
        {
            return null;
        }
        
        public void Add(IInterception interceptor)
        {

        }

    }

}