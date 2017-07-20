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

namespace CatLib.API.Converters
{
    /// <summary>
    /// 转换管理器
    /// </summary>
    public interface IConvertersManager : ISingleManager<IConverters>
    {
        /// <summary>
        /// 克隆指定转换器(注意克隆只克隆解决器提供的结果)
        /// </summary>
        /// <param name="newExtendName">新的名字</param>
        /// <param name="cloneFromExtendName">克隆自的管理器名字</param>
        /// <returns>转换器</returns>
        IConverters CloneExtend(string newExtendName, string cloneFromExtendName = null);
    }
}
