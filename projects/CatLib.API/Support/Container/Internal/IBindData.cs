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

using System;

namespace CatLib
{
    /// <summary>
    /// 服务绑定数据
    /// </summary>
    public interface IBindData
    {
        /// <summary>
        /// 服务名
        /// </summary>
        string Service { get; }

        /// <summary>
        /// 服务实现
        /// </summary>
        Func<IContainer, object[], object> Concrete { get; }

        /// <summary>
        /// 是否是静态服务
        /// </summary>
        bool IsStatic { get; }

        /// <summary>
        /// 当需求某个服务                                                                                                                                                                                                                                                                                                                                                                                  
        /// </summary>
        /// <param name="service">服务名</param>
        /// <returns>绑定关系临时数据</returns>
        IGivenData Needs(string service);

        /// <summary>
        /// 当需求某个服务
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <returns>绑定关系临时数据</returns>
        IGivenData Needs<T>();

        /// <summary>
        /// 为服务设定一个别名
        /// </summary>
        /// <typeparam name="T">别名</typeparam>
        /// <returns>服务绑定数据</returns>
        IBindData Alias<T>();

        /// <summary>
        /// 为服务设定一个别名
        /// </summary>
        /// <param name="alias">别名</param>
        /// <returns>服务绑定数据</returns>
        IBindData Alias(string alias);

        /// <summary>
        /// 解决服务时触发的回调
        /// </summary>
        /// <param name="func">解决事件</param>
        /// <returns>服务绑定数据</returns>
        IBindData OnResolving(Func<IBindData, object, object> func);

        /// <summary>
        /// 当服务被释放时
        /// </summary>
        /// <param name="action">处理事件</param>
        /// <returns>服务绑定数据</returns>
        IBindData OnRelease(Action<IBindData, object> action);

        /// <summary>
        /// 移除绑定服务 , 在解除绑定时如果是静态化物体将会触发释放
        /// </summary>
        void UnBind();
    }
}
