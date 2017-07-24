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

using System.Collections.Generic;
using UnityEngine;

namespace CatLib.Bootstrap
{
    /// <summary>
    /// 程序入口
    /// Program Entry
    /// </summary>
    public sealed class Program : MonoBehaviour
    {
        /// <summary>
        /// 初始化程序
        /// </summary>
        public void Awake()
        {
            var app = new Application(this);
            app.Bootstrap(GetBootstraps());
            app.Init();
        }

        /// <summary>
        /// 获取引导程序
        /// </summary>
        /// <returns>引导脚本</returns>
        private IBootstrap[] GetBootstraps()
        {
            var bootstraps = new List<IBootstrap>();
            bootstraps.AddRange(GetComponents<IBootstrap>());
            bootstraps.AddRange(Bootstrap.BootStrap);
            return bootstraps.ToArray();
        }
    }
}