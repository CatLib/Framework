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

namespace CatLib.API.Container
{

    public interface IBindData
    {

        /// <summary>
        /// 服务名
        /// </summary>
        string Service { get; }

        /// <summary>
        /// 服务实体
        /// </summary>
        Func<IContainer, object[], object> Concrete { get; }

        /// <summary>
        /// 是否是静态的
        /// </summary>
        bool IsStatic { get; }

        /// <summary>
        /// 当需求什么服务
        /// </summary>
        IGivenData Needs(string service);

        /// <summary>
        /// 当需求什么服务
        /// </summary>
        IGivenData Needs<T>();

        /// <summary>
        /// 添加拦截器
        /// </summary>
        IBindData AddInterceptor<T>() where T : IInterception, new();

        /// <summary>
        /// 服务别名
        /// </summary>
        IBindData Alias<T>();

        /// <summary>
        /// 服务别名
        /// </summary>
        IBindData Alias(string alias);

        /// <summary>
        /// 当解决这个服务时
        /// </summary>
        IBindData OnResolving(Func<object, object> func);

    }
}
