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
