using UnityEngine;
using System.Collections;

namespace CatLib.Support
{
    public static class CArr
    {

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