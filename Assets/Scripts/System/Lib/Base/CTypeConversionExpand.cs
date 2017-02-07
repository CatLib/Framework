using System.Collections.Generic;
using System;
using XLua;

namespace CatLib.Base
{
    /// <summary>类型转换拓展方法</summary>
    public static class TypeConversionExpand
    {

        /// <summary>字符串转Byte</summary>
        /// <param name="val">值</param>
        /// <returns></returns>
        public static byte[] ToByte(this string val)
        {
            return System.Text.Encoding.UTF8.GetBytes(val);
        }

        /// <summary>Byte转字符串</summary>
        /// <param name="val">值</param>
        /// <returns></returns>
        public static string String(this byte[] val)
        {
            if (val == null || val.Length <= 0) { return string.Empty; }
            return System.Text.Encoding.UTF8.GetString(val);
        }

        /// <summary>转换为枚举(该函数效率低下)</summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="val">值</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns></returns>
        public static T ToEnum<T>(this int val, bool ignoreCase = true)
        {
            return (T)Enum.Parse(typeof(T), val.ToString(), ignoreCase);
        }

        /// <summary>转换为枚举(该函数效率低下)</summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="val">值</param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns></returns>
        public static T ToEnum<T>(this string val, bool ignoreCase = true)
        {
            return (T)Enum.Parse(typeof(T), val, ignoreCase);
        }

        /// <summary>将枚举转为Int内容</summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToInt(this Enum value)
        {
            Type enumType = value.GetType();
            string name = Enum.GetName(enumType, value);
            if (name != null)
            {
                System.Reflection.FieldInfo fieldInfo = enumType.GetField(name);
                if (fieldInfo != null)
                {
                    return (int)fieldInfo.GetValue(null);
                }
            }
            return 0;
        }

        /// <summary>将字典转为数组(忽略Key)</summary>
        /// <typeparam name="TK"></typeparam>
        /// <typeparam name="TV"></typeparam>
        /// <param name="datas">字典</param>
        /// <returns></returns>
        public static TV[] ToArray<TK, TV>(this System.Collections.Generic.Dictionary<TK, TV> datas)
        {
            List<TV> returnList = new List<TV>();
            returnList.AddRange(datas.Values);
            return returnList.ToArray();
        }

        /// <summary>时间戳转时间</summary>
        /// <param name="val">时间戳</param>
        /// <returns></returns>
        public static DateTime ToTime(this int val)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(val + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        /// <summary>转换为16进制字符串</summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string ToHex(this byte[] bytes)
        {
            string byteStr = string.Empty;
            if (bytes != null || bytes.Length > 0)
            {
                foreach (var item in bytes)
                {
                    byteStr += string.Format("{0:X2}", item);
                }
            }
            return byteStr;
        }


    }

}