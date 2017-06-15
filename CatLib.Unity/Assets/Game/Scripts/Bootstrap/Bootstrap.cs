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

using System;

namespace CatLib.Bootstrap
{
    /// <summary>
    /// 引导程序
    /// </summary>
    public class Bootstrap
    {
        /// <summary>
        /// 引导程序
        /// 请不要随意调整引导顺序，除非您非常了解启动流程
        /// </summary>
        public static Type[] BootStrap
        {
            get
            {
                return new[]
                {
                    typeof(ProvidersBootstrap),
                    typeof(ConfigBootstrap),
                    typeof(StartBootstrap)
                };
            }
        }
    }
}