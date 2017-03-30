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
 
using UnityEngine;

namespace CatLib
{
    public static class ArrHelper
    {

        public static T[] Merge<T>(this T[] first, T[] second)
        {
            T[] result = new T[first.Length + second.Length];
            first.CopyTo(result, 0);
            second.CopyTo(result, first.Length);
            return result;
        }

        /// <summary>
        /// 数组转换至指定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T[] To<T>(this Object[] data) where T : Object
        {
            T[] returnList = new T[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                returnList[i] = data[i] as T;
            }
            return returnList;
        }

        /// <summary>
        /// 数组转换至指定类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T[] To<T>(this object[] data)
        {
            T[] returnList = new T[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                returnList[i] = (T)data[i];
            }
            return returnList;
        }
    }
}