
using System;

namespace CatLib.API.Routing
{

    /// <summary>
    /// 注册路由接口
    /// </summary>
    public interface IRegister
    {

        /// <summary>
        /// 注册一个路由方案
        /// </summary>
        /// <param name="uri">路由地址</param>
        /// <param name="action">行为</param>
        /// <returns></returns>
        IRoutingBind Reg(string uri, Action<IRequest , IResponse> action);

        /// <summary>
        /// 为所有uri的指定参数定义模式，定义模式后只有符合模式后才会被路由
        /// </summary>
        /// <param name="key"></param>
        /// <param name="pattern"></param>
        IRegister Pattern(string key, string pattern);

        /// <summary>
        /// 当路由没有找到时
        /// </summary>
        /// <param name="middleware"></param>
        /// <returns></returns>
        IRegister OnNotFound(Func<IRequest, bool> middleware);

        /// <summary>
        /// 当路由出现错误时
        /// </summary>
        /// <param name="middleware"></param>
        /// <returns></returns>
        IRegister OnError(Func<IRequest, System.Exception , bool> middleware);

    }

}