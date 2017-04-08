
using System.Reflection;
using System.Collections.Generic;

using System;

namespace CatLib.Container
{

    /// <summary>
    /// 参数容器
    /// </summary>
    public class ParameterCollection
    {

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

        private readonly List<ArgumentInfo> argumentInfo;

        private readonly object[] arguments;

        /// <summary>
        /// 构建一个参数容器
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="argumentInfo"></param>
        /// <param name="isArgumentPartOfCollection"></param>
        public ParameterCollection(object[] arguments, ParameterInfo[] argumentInfo, Predicate<ParameterInfo> isArgumentPartOfCollection)
        {
            this.arguments = arguments;
            this.argumentInfo = new List<ArgumentInfo>();
            for (int argumentNumber = 0; argumentNumber < argumentInfo.Length; ++argumentNumber)
            {
                if (isArgumentPartOfCollection(argumentInfo[argumentNumber]))
                {
                    this.argumentInfo.Add(new ArgumentInfo(argumentNumber, argumentInfo[argumentNumber]));
                }
            }
        }

        /// <summary>
        /// 根据下标获取参数
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public object this[int index]
        {
            get { return arguments[argumentInfo[index].Index]; }
            set { arguments[argumentInfo[index].Index] = value; }
        }

        /// <summary>
        /// 根据参数名获取对象
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public object this[string parameterName]
        {
            get { return arguments[argumentInfo[IndexForInputParameterName(parameterName)].Index]; }

            set { arguments[argumentInfo[IndexForInputParameterName(parameterName)].Index] = value; }
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