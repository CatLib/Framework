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

namespace CatLib
{
    /// <summary>
    /// 门面
    /// </summary>
    public abstract class Facade<T>
    {
        /// <summary>
        /// 门面实例
        /// </summary>
        public static T Instance
        {
            get
            {
                return App.Make<T>();
            }
        }
    }
}