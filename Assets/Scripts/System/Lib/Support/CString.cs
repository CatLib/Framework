using UnityEngine;
using System.Collections;

namespace CatLib.Support
{
    public static class CString
    {

        /// <summary>
        /// 字符串后缀
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Suffix(this string str)
        {
            string[] array = str.Split(new char[] { '.' });
            if (array.Length <= 0) { return string.Empty; }
            return '.' + array[array.Length - 1];
        }

    }

}