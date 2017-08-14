/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

namespace CatLib
{
    /// <summary>
    /// 绑定关系临时数据,用于支持链式调用
    /// </summary>
    internal sealed class GivenData : IGivenData
    {
        /// <summary>
        /// 绑定数据
        /// </summary>
        private readonly BindData bindData;
        
        /// <summary>
        /// 服务容器
        /// </summary>
        private readonly Container container;

        /// <summary>
        /// 需求什么服务
        /// </summary>
        private string needs;

        /// <summary>
        /// 绑定关系临时数据
        /// </summary>
        /// <param name="container">服务容器</param>
        /// <param name="bindData">服务绑定数据</param>
        internal GivenData(Container container, BindData bindData)
        {
            this.bindData = bindData;
            this.container = container;
        }

        /// <summary>
        /// 需求什么服务
        /// </summary>
        /// <param name="needs">需求什么服务</param>
        /// <returns>绑定关系实例</returns>
        internal IGivenData Needs(string needs)
        {
            this.needs = needs;
            return this;
        }

        /// <summary>
        /// 给与什么服务
        /// </summary>
        /// <param name="service">给与的服务名或别名</param>
        /// <returns>服务绑定数据</returns>
        public IBindData Given(string service)
        {
            Guard.NotEmptyOrNull(service , "service");
            return bindData.AddContextual(needs, service);
        }

        /// <summary>
        /// 给与什么服务
        /// </summary>
        /// <typeparam name="T">给与的服务名或别名</typeparam>
        /// <returns>服务绑定数据</returns>
        public IBindData Given<T>()
        {
            return Given(container.Type2Service(typeof(T)));
        }
    }
}