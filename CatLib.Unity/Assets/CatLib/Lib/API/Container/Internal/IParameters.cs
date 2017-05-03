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

using System.Reflection;
using System.Collections;

namespace CatLib.API.Container
{
    /// <summary>
    /// 参数容器
    /// </summary>
    public interface IParameters : IEnumerable
    {
        /// <summary>
        /// 根据下标获取参数内容
        /// </summary>
        /// <param name="index">参数下标</param>
        /// <returns>参数内容</returns>
        object this[int index] { get; }

        /// <summary>
        /// 根据参数名获取参数内容
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <returns>参数内容</returns>
        object this[string parameterName] { get; }

        /// <summary>
        /// 获取参数信息
        /// </summary>
        /// <param name="index">参数下标</param>
        /// <returns>参数信息</returns>
        ParameterInfo GetParameterInfo(int index);

        /// <summary>
        /// 获取参数信息
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <returns>参数信息</returns>
        ParameterInfo GetParameterInfo(string parameterName);

        /// <summary>
        /// 根据下标获取参数名
        /// </summary>
        /// <param name="index">参数下标</param>
        /// <returns>参数名</returns>
        string GetParameterName(int index);

        /// <summary>
        /// 是否包含参数
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <returns>是否包含指定参数名的参数</returns>
        bool Contains(string parameterName);

        /// <summary>
        /// 是否包含参数
        /// </summary>
        /// <param name="value">参数内容</param>
        /// <returns>是否包含指定参数内容的参数</returns>
        bool Contains(object value);
    }
}