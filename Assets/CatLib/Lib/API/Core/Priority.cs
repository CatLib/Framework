
using System;

namespace CatLib.API.Routing
{
    /// <summary>
    /// 优先级标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class PriorityAttribute : Attribute
    {

        /// <summary>
        /// 优先值
        /// </summary>
        public int Priorities { get; protected set; }

        public PriorityAttribute(int priority)
        {
            Priorities = priority;
        }
    }

}