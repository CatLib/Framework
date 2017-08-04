﻿/*
 * This file is part of the CatLib package.
 *
 * (c) Yu Bin <support@catlib.io>
 *
 * For the full copyright and license information, please view the LICENSE
 * file that was distributed with this source code.
 *
 * Document: http://catlib.io/
 */

using UnityEngine;

namespace CatLib
{
    /// <summary>
    /// 默认提供者引导
    /// </summary>
    [ExcludeFromCodeCoverage]
    internal class ProviderBootstrap : IBootstrap
    {
        /// <summary>
        /// 引导程序接口
        /// </summary>
        public void Bootstrap()
        {
            LoadUnityComponentProvider();
            LoadCodeProvider();
        }

        /// <summary>
        /// 加载以代码形式提供的服务提供者
        /// </summary>
        private void LoadCodeProvider()
        {
            foreach (var provider in Providers.ServiceProviders)
            {
                if (!App.IsRegisted(provider))
                {
                    App.Register(provider);
                }
            }
        }

        /// <summary>
        /// 加载Unity组件的服务提供者
        /// </summary>
        private void LoadUnityComponentProvider()
        {
            var root = App.Make<Component>();
            if (root == null)
            {
                return;
            }

            var unityObject = typeof(Object);
            foreach (var provider in root.GetComponents<IServiceProvider>())
            {
                if (provider == null)
                {
                    continue;
                }

                App.Register(provider);

                if (unityObject.IsInstanceOfType(provider))
                {
                    Object.Destroy((Object)provider);
                }
            }
        }
    }
}
