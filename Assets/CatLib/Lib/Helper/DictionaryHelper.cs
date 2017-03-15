using System;
using System.Collections.Generic;

namespace CatLib
{

    /// <summary>
    /// 字典辅助类
    /// </summary>
    public static class DictionaryHelper
    {

        /// <summary>
        /// 根据指定的值来删除内容
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="dict"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Dictionary<T1, T2> RemoveValue<T1, T2>(this Dictionary<T1 , T2> dict, T2 value)
        {
            //todo 待测试
            List<T1> removeKey = new List<T1>();
            foreach(KeyValuePair<T1 , T2> data in dict)
            {
                if(data.Value.Equals(value))
                {
                    removeKey.Add(data.Key);
                }
            }
            foreach(T1 key in removeKey)
            {
                dict.Remove(key);
            }
            return dict;
        }

        public static void Walk<T1,T2>(this ICollection<KeyValuePair<T1, T2>> dict , Action<T1, T2> callback)
        {
            foreach(var kv in dict)
            {
                callback(kv.Key, kv.Value);
            }
        }

    }

}