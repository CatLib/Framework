
namespace CatLib.Base
{
    /// <summary>
    /// 触发器等级
    /// </summary>
    public enum TriggerLevel
    {

        /// <summary>
        /// 不通知
        /// </summary>
        NONE = 0,

        /// <summary>
        /// 自身对象级通知
        /// </summary>
        SELF = 1 << 1,

        /// <summary>
        /// 类型级通知
        /// </summary>
        TYPE = 1 << 2,

        /// <summary>
        /// 全局通知
        /// </summary>
        GLOBAL = 1 << 3,

        /// <summary>
        /// 全部通知
        /// </summary>
        ALL = int.MaxValue,
        

    }

}