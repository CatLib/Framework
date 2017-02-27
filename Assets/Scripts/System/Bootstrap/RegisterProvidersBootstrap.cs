using System;
using CatLib.API;

namespace CatLib
{

    /// <summary>
    /// 注册服务提供商的引导程序
    /// </summary>
    public class RegisterProvidersBootstrap : IBootstrap
    {

        /// <summary>
        /// 引导程序
        /// </summary>
        public void Bootstrap()
        {
            foreach(Type t in Providers.ServiceProviders)
            {
                App.Instance.Register(t);
            }
        }
    }

}