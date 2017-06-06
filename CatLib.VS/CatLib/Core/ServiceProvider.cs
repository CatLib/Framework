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
        /// CatLib实例
        /// </summary>
        public IApplication App
        {
            get
            {
                return CatLib.App.Instance;
            }
        }

        /// <summary>
        /// 服务提供者初始化
        /// </summary>
        /// <returns>迭代器</returns>
        public virtual IEnumerator Init()
        {
            yield break;
        }

        /// <summary>
        /// 当注册服务提供商
        /// </summary>
        public abstract void Register();
    }
}