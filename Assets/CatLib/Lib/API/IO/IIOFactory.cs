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
    /// IO工厂
    /// </summary>
    public interface IIOFactory
    {

        /// <summary>
        /// 获取文件驱动器
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IDisk Disk(string name = null);

    }
}