using System;

namespace CatLib.API
{
    /// <summary>
    /// 标记的类，函数，属性不再进行覆盖率分析
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Field, Inherited = false)]
    public class ExcludeFromCodeCoverageAttribute : Attribute
    {
    }
}
