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

namespace CatLib.API.IO
{
    /// <summary>
    /// IO
    /// </summary>
    public interface IIOFactory
    {
        /// <summary>
        /// 获取磁盘驱动器
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>驱动器</returns>
		IDisk Disk(string name = null);
    }
}