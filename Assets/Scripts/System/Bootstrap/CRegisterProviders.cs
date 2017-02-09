using System;
using CatLib.Contracts.Base;
using CatLib.Base;

namespace CatLib
{

    /// <summary>
    /// 注册服务提供商
    /// </summary>
    public class CRegisterProviders : IBootstrap
    {

        /// <summary>
        /// 引导程序
        /// </summary>
        public void Bootstrap()
        {
            foreach(Type t in CProviders.ServiceProviders)
            {
                CApp.Instance.Register(t);
            }
        }
    }

}