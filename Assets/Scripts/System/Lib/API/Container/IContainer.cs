using UnityEngine;
using System.Collections;
using System;
using XLua;

namespace CatLib.API.Container
{
	///<summary>容器接口</summary>
    public interface IContainer
    {

        /// <summary>
        /// 绑定一个服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务实体</param>
        /// <param name="isStatic">服务是否是静态的</param>
        /// <returns></returns>
        IBindData Bind(string service, Func<IContainer, object[], object> concrete, bool isStatic);

        /// <summary>
        /// 绑定一个服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务实体</param>
        /// <param name="isStatic">服务是否是静态的</param>
        /// <returns></returns>
        IBindData Bind(string service, string concrete, bool isStatic);

        /// <summary>
        /// 生成一个绑定服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        object Make(string service, params object[] param);

        /// <summary>
        /// 别名
        /// </summary>
        /// <param name="alias">别名</param>
        /// <param name="service">提供的服务名</param>
        /// <returns></returns>
        IContainer Alias(string alias, string service);

        /// <summary>
        /// 修饰器
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        IContainer Decorator(Func<IContainer, IBindData, object, object> func);

    }
}