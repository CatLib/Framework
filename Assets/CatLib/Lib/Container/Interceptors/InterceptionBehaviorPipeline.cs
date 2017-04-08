
using System.Collections.Generic;

namespace CatLib.Container
{

    /// <summary>
    /// 拦截器管道
    /// </summary>
    public class InterceptionBehaviorPipeline
    {

        private readonly List<IInterceptionBehavior> interceptionBehaviors;

        public InterceptionBehaviorPipeline()
        {
            interceptionBehaviors = new List<IInterceptionBehavior>();
        }

        public InterceptionBehaviorPipeline(IEnumerable<IInterceptionBehavior> interceptionBehaviors)
        {
            this.interceptionBehaviors = new List<IInterceptionBehavior>(interceptionBehaviors);
        }


    }

}