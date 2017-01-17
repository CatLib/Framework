using UnityEngine;
using System.Collections;
using System;

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
                CApplication.Instance.Register(t);
            }
        }
    }

}