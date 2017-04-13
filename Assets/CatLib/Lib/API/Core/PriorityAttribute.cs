
using System;

namespace CatLib.API
{
    /// <summary>
    /// 优先级标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class PriorityAttribute : Attribute
    {

        /// <summary>
        /// 优先值
        /// </summary>
        public int Priorities { get; protected set; }

        public PriorityAttribute(int priority = int.MaxValue)
        {
            Priorities = priority;
        }
    }

}