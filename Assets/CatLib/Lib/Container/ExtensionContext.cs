
using CatLib.API.Container;

namespace CatLib.Container
{
    
    /// <summary>
    /// 扩展上下文
    /// </summary>
    public abstract class ExtensionContext
    {


        /// <summary>
        /// 容器
        /// </summary>
        public abstract IContainer Container { get; }

        /// <summary>
        /// 编译策略链
        /// </summary>
        public abstract BuildStrategyChain<BuildStages> BuildStrategy { get; }


    }

}