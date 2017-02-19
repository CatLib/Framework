
namespace CatLib.Contracts.Time
{

    /// <summary>
    /// 时间任务处理句柄
    /// </summary>
    public interface ITimeTaskHandler
    {

        /// <summary>
        /// 取消时间任务执行
        /// </summary>
        void Cancel();

    }

}