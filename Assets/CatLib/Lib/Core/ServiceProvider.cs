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
 
using System.Collections;
using CatLib.API;

namespace CatLib
{
    /// <summary>
    /// 服务提供者
    /// </summary>
    public abstract class ServiceProvider
    {
        /// <summary>
        /// 服务提供商初始化
        /// </summary>
        public virtual void Init() { }

        /// <summary>
        /// 服务提供商启动流程
        /// </summary>
        public virtual ProviderProcess ProviderProcess
        {
            get { return ProviderProcess.Normal; }
        }

        /// <summary>
        /// 当服务提供商触发启动流程时
        /// </summary>
        /// <returns>迭代器</returns>
        public virtual IEnumerator OnProviderProcess()
        {
            yield break;
        }

        /// <summary>
        /// CatLib实例
        /// </summary>
        public IApplication App
        {
            get { return CatLib.App.Instance; }
        }

        /// <summary>
        /// 当注册服务提供商
        /// </summary>
        public abstract void Register();
    }
}