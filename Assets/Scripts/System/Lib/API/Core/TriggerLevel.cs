
namespace CatLib.API
{
    /// <summary>
    /// 触发器等级
    /// </summary>
    public enum TriggerLevel
    {

        /// <summary>
        /// 不通知
        /// </summary>
        None = 0,

        /// <summary>
        /// 自身对象级通知
        /// </summary>
        Self = 1 << 1,

        /// <summary>
        /// 类型级通知
        /// </summary>
        Type = 1 << 2,

        /// <summary>
        /// 接口级通知
        /// </summary>
        Interface = 1 << 3,

        /// <summary>
        /// 全局通知
        /// </summary>
        Global = 1 << 4,

        /// <summary>
        /// 全部通知
        /// </summary>
        All = int.MaxValue,
        

    }

}