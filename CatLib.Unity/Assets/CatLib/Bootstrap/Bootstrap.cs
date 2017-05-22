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

namespace CatLib
{
    /// <summary>
    /// 引导程序
    /// </summary>
    public class Bootstrap
    {
        /// <summary>
        /// 引导程序
        /// </summary>
        public static Type[] BootStrap
        {
            get
            {
                return new []
                {
                    typeof(RegisterProvidersBootstrap)
                };
            }

        }
    }
}