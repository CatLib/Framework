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

namespace CatLib.API
{
    /// <summary>
    /// CatLib实例
    /// </summary>
    public sealed class App
    {
        /// <summary>
        /// CatLib实例
        /// </summary>
        private static IApplication instance;

        /// <summary>
        /// CatLib实例
        /// </summary>
        public static IApplication Instance
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }
                throw new NullReferenceException("Application is not instance.");
            }
            set
            {
                instance = value;
            }
        }
    }
}