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

using CatLib.API.Container;

namespace CatLib.Container
{
    /// <summary>
    /// 绑定关系临时数据
    /// </summary>
    public class GivenData : IGivenData
    {
        /// <summary>
        /// 绑定数据
        /// </summary>
        private readonly BindData bindData;

        /// <summary>
        /// 需求什么服务
        /// </summary>
        private readonly string needs;

        /// <summary>
        /// 绑定关系临时数据
        /// </summary>
        public GivenData(BindData bindData, string needs)
        {
            this.bindData = bindData;
            this.needs = needs;
        }

        /// <summary>
        /// 给与什么服务
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <returns></returns>
        public IBindData Given(string service)
        {
            return bindData.AddContextual(needs, service);
        }

        /// <summary>
        /// 给与什么服务
        /// </summary>
        /// <typeparam name="T">服务</typeparam>
        /// <returns></returns>
        public IBindData Given<T>()
        {
            return Given(typeof(T).ToString());
        }
    }
}