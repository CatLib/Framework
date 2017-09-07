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

namespace CatLib.API.Json
{
    /// <summary>
    /// Json实例接口
    /// </summary>
    public interface IJsonAware
    {
        /// <summary>
        /// 设定json处理器实例接口
        /// </summary>
        /// <param name="handler">json处理器</param>
        void SetJson(IJson handler);
    }
}
