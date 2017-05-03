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

namespace CatLib.API
{
    /// <summary>
    /// 唯一标识符接口
    /// </summary>
    public interface IGuid
    {
        /// <summary>
        /// 获取当前类的全局唯一标识符
        /// </summary>
        long Guid { get; }
    }
}