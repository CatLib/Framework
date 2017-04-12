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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using CatLib.API.Container;

namespace CatLib.Container
{
    /// <summary>
    /// 参数容器
    /// </summary>
    public class ParameterCollection : IParameters
    {
        /// <summary>
        /// 参数信息
        /// </summary>
        private struct ArgumentInfo
        {
            /// <summary>
            /// 参数下标
            /// </summary>
            public readonly int Index;

            /// <summary>
            /// 参数名
            /// </summary>
            public readonly string Name;

            /// <summary>
            /// 参数信息
            /// </summary>
            public readonly ParameterInfo ParameterInfo;

            /// <summary>
            /// 构建一个参数信息
            /// </summary>
            /// <param name="index">参数下标</param>
            /// <param name="parameterInfo">参数信息</param>
            public ArgumentInfo(int index, ParameterInfo parameterInfo)
            {
                Index = index;
                Name = parameterInfo.Name;
                ParameterInfo = parameterInfo;
            }
        }

        /// <summary>
        /// 参数信息列表
        /// </summary>
        private readonly List<ArgumentInfo> argumentInfo;

        /// <summary>
        /// 参数内容列表
        /// </summary>
        private readonly object[] arguments;

        /// <summary>
        /// 构建一个参数容器
        /// </summary>
        /// <param name="arguments">参数内容</param>
        /// <param name="argumentInfo">参数信息</param>
        /// <param name="isEffective">参数是否生效</param>
        public ParameterCollection(object[] arguments, IList<ParameterInfo> argumentInfo, Predicate<ParameterInfo> isEffective)
        {
            this.arguments = arguments;
            this.argumentInfo = new List<ArgumentInfo>();
            for (var argumentNumber = 0; argumentNumber < argumentInfo.Count; ++argumentNumber)
            {
                if (isEffective(argumentInfo[argumentNumber]))
                {
                    this.argumentInfo.Add(new ArgumentInfo(argumentNumber, argumentInfo[argumentNumber]));
                }
            }
        }

        /// <summary>
        /// 根据下标获取参数内容
        /// </summary>
        /// <param name="index">参数下标</param>
        /// <returns>参数内容</returns>
        public object this[int index]
        {
            get { return arguments[argumentInfo[index].Index]; }
            set { arguments[argumentInfo[index].Index] = value; }
        }

        /// <summary>
        /// 根据参数名获取参数内容
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <returns>参数内容</returns>
        public object this[string parameterName]
        {
            get { return arguments[argumentInfo[IndexForInputParameterName(parameterName)].Index]; }
            set { arguments[argumentInfo[IndexForInputParameterName(parameterName)].Index] = value; }
        }

        /// <summary>
        /// 获取参数信息
        /// </summary>
        /// <param name="index">参数下标</param>
        /// <returns>参数信息</returns>
        public ParameterInfo GetParameterInfo(int index)
        {
            return argumentInfo[index].ParameterInfo;
        }

        /// <summary>
        /// 获取参数信息
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <returns>参数信息</returns>
        public ParameterInfo GetParameterInfo(string parameterName)
        {
            return argumentInfo[IndexForInputParameterName(parameterName)].ParameterInfo;
        }

        /// <summary>
        /// 根据下标获取参数名
        /// </summary>
        /// <param name="index">参数下标</param>
        /// <returns>参数名</returns>
        public string GetParameterName(int index)
        {
            return argumentInfo[index].Name;
        }

        /// <summary>
        /// 是否包含参数
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <returns>是否包含指定参数名的参数</returns>
        public bool Contains(string parameterName)
        {
            for (var i = 0; i < argumentInfo.Count; i++)
            {
                if (argumentInfo[i].Name == parameterName)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否包含参数
        /// </summary>
        /// <param name="value">参数内容</param>
        /// <returns>是否包含指定参数内容的参数</returns>
        public bool Contains(object value)
        {
            return argumentInfo.Exists(info =>
                    {
                        var argument = arguments[info.Index];

                        if (argument == null)
                        {
                            return value == null;
                        }

                        return argument.Equals(value);
                    });
        }

        /// <summary>
        /// 迭代器
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetEnumerator()
        {
            for (var i = 0; i < argumentInfo.Count; ++i)
            {
                yield return arguments[argumentInfo[i].Index];
            }
        }

        /// <summary>
        /// 获取输入参数的下标
        /// </summary>
        /// <param name="paramName">参数名</param>
        /// <returns>参数下标</returns>
        private int IndexForInputParameterName(string paramName)
        {
            for (var i = 0; i < argumentInfo.Count; ++i)
            {
                if (argumentInfo[i].Name == paramName)
                {
                    return i;
                }
            }
            throw new ArgumentException("Invalid parameter Name", "paramName");
        }
    }
}