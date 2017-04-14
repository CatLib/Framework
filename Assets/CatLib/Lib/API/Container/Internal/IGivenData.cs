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

namespace CatLib.API.Container
{
    /// <summary>
    /// 绑定关系临时数据
    /// </summary>
    public interface IGivenData
    {
        /// <summary>
        /// 给与什么服务
        /// </summary>
        /// <param name="service">服务名或别名</param>
        /// <returns>服务绑定数据</returns>
        IBindData Given(string service);

        /// <summary>
        /// 给与什么服务
        /// </summary>
        /// <typeparam name="T">服务名</typeparam>
        /// <returns>服务绑定数据</returns>
        IBindData Given<T>();
    }
}