
using System;

namespace CatLib.API.Routing
{
    /// <summary>
    /// 优先级标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class Priority : Attribute
    {

        /// <summary>
        /// 优先值
        /// </summary>
        public int Priorities { get; protected set; }

        public Priority(int priority)
        {
            Priorities = priority;
        }
    }

}