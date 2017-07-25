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

using UnityEngine;

namespace CatLib.Bootstrap
{
    /// <summary>
    /// 注册服务提供商的引导程序
    /// </summary>
    public class ProvidersBootstrap : IBootstrap
    {
        /// <summary>
        /// 引导程序
        /// </summary>
        public void Bootstrap()
        {
            LoadCodeProvider();
            LoadUnityComponentProvider();
        }

        /// <summary>
        /// 加载以代码形式提供的服务提供者
        /// </summary>
        private void LoadCodeProvider()
        {
            foreach (var type in Providers.ServiceProviders)
            {
                App.Register(type);
            }
        }

        /// <summary>
        /// 加载以Unity组件构建的服务提供者
        /// </summary>
        private void LoadUnityComponentProvider()
        {
            var root = App.Make<Component>();
            if (root != null)
            {
                var unityObject = typeof(Object);
                var serviceProviders = root.GetComponents<IServiceProvider>();
                foreach (var serviceProvider in serviceProviders)
                {
                    if (serviceProvider == null)
                    {
                        continue;
                    }

                    App.Register(serviceProvider);

                    if (unityObject.IsInstanceOfType(serviceProvider))
                    {
                        Object.Destroy((Object)serviceProvider);
                    }
                }
            }
        }
    }
}