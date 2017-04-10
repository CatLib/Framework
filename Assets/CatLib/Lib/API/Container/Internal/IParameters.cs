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
    /// 参数
    /// </summary>
    public interface IParameters : IEnumerable
    {

        object this[int index] { get; }

        object this[string parameterName] { get; }

        ParameterInfo GetParameterInfo(int index);

        ParameterInfo GetParameterInfo(string parameterName);

        string GetParameterName(int index);

        bool Contains(string parameterName);

        bool Contains(object value);

    }

}