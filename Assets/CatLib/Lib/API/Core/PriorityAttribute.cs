
using System;

namespace CatLib.API
{
    /// <summary>
    /// 执行优先级
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class PriorityAttribute : Attribute
    {
        /// <summary>
        /// 优先值
        /// </summary>
        public int Priorities { get; protected set; }

        /// <summary>
        /// 执行优先级
        /// </summary>
        /// <param name="priority">优先级(0为最优先)</param>
        public PriorityAttribute(int priority = int.MaxValue)
        {
            Priorities = priority;
        }
    }
}