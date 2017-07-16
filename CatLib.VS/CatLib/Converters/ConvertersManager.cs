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

using CatLib.API.Converters;
using CatLib.Stl;

namespace CatLib.Converters
{
    /// <summary>
    /// 转换器管理器
    /// </summary>
    internal sealed class ConvertersManager : SingleManager<IConverters> , IConvertersManager
    {
        /// <summary>
        /// 克隆指定转换器(注意克隆只克隆解决器提供的结果)
        /// </summary>
        /// <param name="newExtendName">新的名字</param>
        /// <param name="cloneFromExtendName">克隆自的管理器名字</param>
        /// <returns>转换器</returns>
        public IConverters CloneExtend(string newExtendName, string cloneFromExtendName = null)
        {
            var resolve = GetResolve(cloneFromExtendName);
            Extend(resolve, newExtendName);
            return Get(newExtendName);
        }
    }
}
