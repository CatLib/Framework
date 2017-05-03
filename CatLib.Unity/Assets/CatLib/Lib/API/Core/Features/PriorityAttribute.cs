
using System;

namespace CatLib.API
{
    /// <summary>
    /// 执行优先级
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class PriorityAttribute : Attribute
    {
        /// <summary>
        /// 优先级
        /// </summary>
        public int Priorities { get; private set; }

        /// <summary>
        /// 优先级(0最高)
        /// </summary>
        /// <param name="priority">优先级(0为最优先)</param>
        public PriorityAttribute(int priority = int.MaxValue)
        {
            Priorities = Math.Max(priority, 0);
        }
    }
}