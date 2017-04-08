
using System.Reflection;
using System.Collections.Generic;
using System.Collections;
using System;
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
            public int Index;
            public string Name;
            public ParameterInfo ParameterInfo;

            public ArgumentInfo(int index, ParameterInfo parameterInfo)
            {
                Index = index;
                Name = parameterInfo.Name;
                ParameterInfo = parameterInfo;
            }
        }

        /// <summary>
        /// 参数信息
        /// </summary>
        private readonly List<ArgumentInfo> argumentInfo;

        /// <summary>
        /// 参数实体
        /// </summary>
        private readonly object[] arguments;

        /// <summary>
        /// 构建一个参数容器
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="argumentInfo"></param>
        /// <param name="isEffective">参数是否生效</param>
        public ParameterCollection(object[] arguments, ParameterInfo[] argumentInfo, Predicate<ParameterInfo> isEffective)
        {
            this.arguments = arguments;
            this.argumentInfo = new List<ArgumentInfo>();
            for (int argumentNumber = 0; argumentNumber < argumentInfo.Length; ++argumentNumber)
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
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get { return arguments[argumentInfo[index].Index]; }
            set { arguments[argumentInfo[index].Index] = value; }
        }

        /// <summary>
        /// 根据参数名获取参数内容
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public object this[string parameterName]
        {
            get { return arguments[argumentInfo[IndexForInputParameterName(parameterName)].Index]; }

            set { arguments[argumentInfo[IndexForInputParameterName(parameterName)].Index] = value; }
        }

        /// <summary>
        /// 获取参数信息
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public ParameterInfo GetParameterInfo(int index)
        {
            return argumentInfo[index].ParameterInfo;
        }

        /// <summary>
        /// 获取参数信息
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public ParameterInfo GetParameterInfo(string parameterName)
        {
            return argumentInfo[IndexForInputParameterName(parameterName)].ParameterInfo;
        }

        /// <summary>
        /// 根据下标获取参数名
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string GetParameterName(int index)
        {
            return argumentInfo[index].Name;
        }

        /// <summary>
        /// 是否包含参数
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public bool Contains(string parameterName)
        {
            for(int i = 0; i < argumentInfo.Count; i++)
            {
                if(argumentInfo[i].Name == parameterName)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 是否包含
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(object value)
        {
            return argumentInfo.Exists((ArgumentInfo info) =>
                    {
                        var argument = arguments[info.Index];

                        if (argument == null)
                        {
                            return value == null;
                        }

                        return argument.Equals(value);
                    });
        }


        public IEnumerator GetEnumerator()
        {
            for (int i = 0; i < argumentInfo.Count; ++i)
            {
                yield return arguments[argumentInfo[i].Index];
            }
        }


        /// <summary>
        /// 获取输入参数的下表
        /// </summary>
        /// <param name="paramName"></param>
        /// <returns></returns>
        private int IndexForInputParameterName(string paramName)
        {
            for (int i = 0; i < argumentInfo.Count; ++i)
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