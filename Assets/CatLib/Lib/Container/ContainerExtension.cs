
using CatLib.API.Container;
using System;

namespace CatLib.Container
{

    /// <summary>
    /// 容器扩展
    /// </summary>
    public abstract class ContainerExtension
    {

        private IContainer container;
        private ExtensionContext context;

        public void InitializeExtension(ExtensionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            container = context.Container;
            this.context = context;
            Initialize();
        }

        /// <summary>
        /// 容器
        /// </summary>
        public IContainer Container
        {
            get { return this.container; }
        }

        /// <summary>
        /// 扩展上下文
        /// </summary>
        protected ExtensionContext Context
        {
            get { return context; }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected abstract void Initialize();

        /// <summary>
        /// 当扩展被移除时
        /// </summary>
        public virtual void Remove(){ }

    }

}