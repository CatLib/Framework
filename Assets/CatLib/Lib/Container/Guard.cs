
using System;
using System.Collections;
using System.Collections.Generic;

namespace CatLib.Container
{
    /// <summary>
    /// 守卫 , 保证输入值都是合法的
    /// </summary>
    internal sealed class Guard
    {
        /// <summary>
        /// 字符串不空null或者空
        /// </summary>
        /// <param name="argumentValue">参数值</param>
        /// <param name="argumentName">参数名</param>
        public static void NotEmptyOrNull(string argumentValue, string argumentName)
        {
            if (string.IsNullOrEmpty(argumentValue))
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        /// <summary>
        /// 长度大于0
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="argumentValue">参数值</param>
        /// <param name="argumentName">参数名</param>
        public static void CountGreaterZero<T>(IList<T> argumentValue, string argumentName)
        {
            if (argumentValue.Count <= 0)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        /// <summary>
        /// 元素部位空或者null
        /// </summary>
        /// <param name="argumentValue">参数值</param>
        /// <param name="argumentName">参数名</param>
        public static void ElementNotEmptyOrNull(IList<string> argumentValue, string argumentName)
        {
            foreach (var val in argumentValue)
            {
                if (string.IsNullOrEmpty(val))
                {
                    throw new ArgumentNullException(argumentName, "Argument element can not be empty or null");
                }
            }
        }

        /// <summary>
        /// 内容不为空
        /// </summary>
        /// <param name="argumentValue">参数值</param>
        /// <param name="argumentName">参数名</param>
        public static void NotNull(object argumentValue, string argumentName)
        {
            if (argumentValue == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }
    }
}
