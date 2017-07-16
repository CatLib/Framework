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
    }
}
