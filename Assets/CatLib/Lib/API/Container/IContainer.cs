using System;

namespace CatLib.API.Container
{
	///<summary>容器接口</summary>
    public interface IContainer
    {

        /// <summary>
        /// 获取服务的绑定数据
        /// </summary>
        /// <param name="service">服务名</param>
        /// <returns></returns>
        IBindData GetBind(string service);

        /// <summary>
        /// 给定的服务是否被绑定
        /// </summary>
        /// <param name="service">服务名</param>
        /// <returns></returns>
        bool HasBind(string service);

        /// <summary>
        /// 给定的服务是否是一个静态服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <returns></returns>
        bool IsStatic(string service);

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
        /// 如果服务不存在那么绑定
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务实体</param>
        /// <param name="isStatic">服务是否是静态的</param>
        /// <returns></returns>
        IBindData BindIf(string service, Func<IContainer, object[], object> concrete, bool isStatic);

        /// <summary>
        /// 如果服务不存在那么绑定
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="concrete">服务实体</param>
        /// <param name="isStatic">服务是否是静态的</param>
        /// <returns></returns>
        IBindData BindIf(string service, string concrete, bool isStatic);

        /// <summary>
        /// 手动注册服务实例
        /// </summary>
        /// <param name="service"></param>
        /// <param name="instance"></param>
        void Instance(string service, object instance);

        /// <summary>
        /// 以依赖注入形式调用一个方法
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="method"></param>
        /// <param name="param"></param>
        object Call(object instance , string method, params object[] param);

        /// <summary>
        /// 生成一个绑定服务
        /// </summary>
        /// <param name="service">服务名</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        object Make(string service, params object[] param);

        /// <summary>
        /// 生成一个绑定服务
        /// </summary>
		object this[string service]{ get; }

        /// <summary>
        /// 别名
        /// </summary>
        /// <param name="alias">别名</param>
        /// <param name="service">提供的服务名</param>
        /// <returns></returns>
        IContainer Alias(string alias, string service);

        /// <summary>
        /// 当解析对象时触发的事件
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        IContainer Resolving(Func<IContainer, IBindData, object, object> func);

    }
}