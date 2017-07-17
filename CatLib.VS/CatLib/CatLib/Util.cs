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
using System.Collections.Generic;

namespace CatLib
{
    /// <summary>
    /// 公共方法
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// 查找实现接口的类型
        /// </summary>
        /// <param name="interfaceType">接口类型</param>
        /// <returns>实现接口的类型列表</returns>
        public static Type[] FindTypesWithInterface(Type interfaceType)
        {
            var lstType = new List<Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var t in type.GetInterfaces())
                    {
                        if (t == interfaceType)
                        {
                            lstType.Add(type);
                        }
                    }
                }
            }
            return lstType.ToArray();
        }

        /// <summary>
        /// 标准化路径
        /// </summary>
        public static string StandardPath(string path)
        {
            return path.Replace("\\", "/");
        }
    }
}

